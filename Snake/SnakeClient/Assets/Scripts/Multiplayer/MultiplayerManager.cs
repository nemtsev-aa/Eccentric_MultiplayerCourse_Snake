using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>, IService, CustomEventBus.IDisposable {
    [SerializeField] private PlayerAim _playerAim;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;

    [SerializeField] private Apple _applePrefab;
    private Dictionary<Vector2Float, Apple> _apples = new Dictionary<Vector2Float, Apple>();
    private const string _gameRoomName = "state_handler";
    private ColyseusRoom<State> _room;
 
    private EventBus _eventBus;
    private PlayerSettings _playerSettings; 
    private SkinsManager _skinsManager;

    #region Server
    public void Init(EventBus eventBus, PlayerSettings playerSettings, SkinsManager skinsManager) {
        _eventBus = eventBus;
        _playerSettings = playerSettings;
        _skinsManager = skinsManager;

        base.Awake();
        InitializeClient();

        _eventBus.Subscribe((GameStartStateSignal signal) => Connection());
    }

    public async void Connection() {

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"sId", _playerSettings.SkinId },
            {"login", _playerSettings.Login }
        };

        _room = await client.JoinOrCreate<State>(_gameRoomName, data);
        _room.OnStateChange += OnChanged;
    }

    private void OnChanged(State state, bool isFirstState) {
        if (isFirstState == false) return;
        _room.OnStateChange -= OnChanged;

        state.players.ForEach((key, player) => {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;

        _room.State.apples.ForEach(CreateApple);
        _room.State.apples.OnAdd += (key, apple) => CreateApple(apple);
        _room.State.apples.OnRemove += RemoveApple;
    }

    public void SendMessage(string key, Dictionary<string, object> data) {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data) {
        _room.Send(key, data);
    }

    protected override void OnApplicationQuit() {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public void LeaveRoom() {
        _room?.Leave();
    }

    public void Dispose() {
        _eventBus.Unsubscribe((GameStartStateSignal signal) => Connection());
    }
    #endregion

    #region Player
    private void CreatePlayer(Player player) {
        Vector3 position = new Vector3(player.x, 0f, player.z);
        Quaternion rotation = Quaternion.identity;
        Snake snake = Instantiate(_snakePrefab, position, rotation);

        SkinData skin = _skinsManager.GetSkin(player.sId);
        snake.Init(player.d, skin, true);

        PlayerAim playerAim = Instantiate(_playerAim, position, rotation);
        playerAim.Init(snake.GetHeadTransform, snake.Speed);

        Controller controller = Instantiate(_controllerPrefab);
        controller.Init(_room.SessionId, playerAim, player, snake);

        PointerManager.Instance.SetPlayerTransform(playerAim.transform);

        _eventBus.Invoke(new AddLeaderSignal(_room.SessionId, player));
    }
    #endregion

    #region Enemy
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();
    private void CreateEnemy(string key, Player player) {
        Vector3 position = new Vector3(player.x, 0f, player.z);
        Snake snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        SkinData skin = _skinsManager.GetSkin(player.sId);
        snake.Init(player.d, skin);

        EnemyController newEnemy = snake.gameObject.AddComponent<EnemyController>();
        EnemyHealth enemyHealth = snake.gameObject.AddComponent<EnemyHealth>();
        EnemyPointer enemyPointer = snake.gameObject.AddComponent<EnemyPointer>();
        enemyPointer.Init(enemyHealth);
        newEnemy.Init(key, player, snake);
        _enemies.Add(key, newEnemy);

        _eventBus.Invoke(new AddLeaderSignal(key, player));
    }

    private void RemoveEnemy(string key, Player value) {

        _eventBus.Invoke(new RemoveLeaderSignal(key));

        if (_enemies.ContainsKey(key) == false || _enemies.Count == 0) {
            Debug.LogError("ѕопытка удалени€ врага, отсутствующегов списке!");
            return;
        }

        EnemyController enemy = _enemies[key];
        PointerManager.Instance.RemoveFromList(enemy.transform.GetComponent<EnemyPointer>());
        enemy.Destroy();
        _enemies.Remove(key);
    }
    #endregion

    #region Apple
    private void CreateApple(Vector2Float vector2Float) {
        Vector3 position = new Vector3(vector2Float.x, 0f, vector2Float.z);
        Apple apple = Instantiate(_applePrefab, position, Quaternion.identity);
        apple.Init(vector2Float);
        _apples.Add(vector2Float, apple);
    }

    private void RemoveApple(int key, Vector2Float vector2Float) {
        if (_apples.ContainsKey(vector2Float) == false) return;

        Apple apple = _apples[vector2Float];
        _apples.Remove(vector2Float);
        apple.Destroy();
    }


    #endregion

}
