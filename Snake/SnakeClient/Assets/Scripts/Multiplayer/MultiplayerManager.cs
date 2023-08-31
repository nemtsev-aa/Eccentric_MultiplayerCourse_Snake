using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using Unity.VisualScripting;
using TMPro;
using System.Linq;

public class MultiplayerManager : ColyseusManager<MultiplayerManager> {
    [field: SerializeField] public SkinsManager Skins { get; private set; }

    [SerializeField] private PlayerAim _playerAim;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;

    [SerializeField] private Apple _applePrefab;
    private Dictionary<Vector2Float, Apple> _apples = new Dictionary<Vector2Float, Apple>();
    private const string _gameRoomName = "state_handler";
    private ColyseusRoom<State> _room;

    #region Server
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeClient();
    }

    public async void Connection() {
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"sId", PlayerSettings.Instance.SkinId },
            {"login", PlayerSettings.Instance.Login }
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
    #endregion

    #region Player
    private void CreatePlayer(Player player) {
        Vector3 position = new Vector3(player.x, 0f, player.z);
        Quaternion rotation = Quaternion.identity;
        Snake snake = Instantiate(_snakePrefab, position, rotation);

        SkinData skin = Skins.GetSkin(player.sId);
        snake.Init(player.d, skin, true);

        PlayerAim playerAim = Instantiate(_playerAim, position, rotation);
        playerAim.Init(snake.GetHeadTransform, snake.Speed);

        Controller controller = Instantiate(_controllerPrefab);
        controller.Init(_room.SessionId, playerAim, player, snake);

        PointerManager.Instance.SetPlayerTransform(playerAim.transform);
        AddLeader(_room.SessionId, player);
    }
    #endregion

    #region Enemy
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();
    private void CreateEnemy(string key, Player player) {
        Vector3 position = new Vector3(player.x, 0f, player.z);
        Snake snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        SkinData skin = Skins.GetSkin(player.sId);
        snake.Init(player.d, skin);

        EnemyController newEnemy = snake.AddComponent<EnemyController>();
        EnemyHealth enemyHealth = snake.AddComponent<EnemyHealth>();
        EnemyPointer enemyPointer = snake.AddComponent<EnemyPointer>();
        enemyPointer.Init(enemyHealth);
        newEnemy.Init(key, player, snake);
        _enemies.Add(key, newEnemy);

        AddLeader(key, player);
    }

    private void RemoveEnemy(string key, Player value) {
        RemoveLeader(key);

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

    #region LeaderBoard
    private class LoginScorePair {
        public string login;
        public float score;
    }

    [SerializeField] private TextMeshProUGUI _text;
    Dictionary<string, LoginScorePair> _leaders = new Dictionary<string, LoginScorePair>();

    private void AddLeader(string sessionId, Player player) {
        if (_leaders.ContainsKey(sessionId)) return;

        _leaders.Add(sessionId, new LoginScorePair {
            login = player.login,
            score = player.score
        });

        UpdateBoard();
    }

    private void RemoveLeader(string sessionID) {
        if (_leaders.ContainsKey(sessionID) == false) return;
        _leaders.Remove(sessionID);

        UpdateBoard();
    }

    public void UpdateScore(string sessionId, int score) {
        if (_leaders.ContainsKey(sessionId) == false) return;

        _leaders[sessionId].score = score;
        UpdateBoard();
    }

    private void UpdateBoard() {
        int topCount = Mathf.Clamp(_leaders.Count, 0, 8);
        var top8 = _leaders.OrderByDescending(pair => pair.Value.score).Take(topCount);
        string text = "";
        int i = 1;
        foreach (var item in top8) {
            text += $"{i}. {item.Value.login}: {item.Value.score}\n";
            i++;
        }
        _text.text = text;
    }


    #endregion
}
