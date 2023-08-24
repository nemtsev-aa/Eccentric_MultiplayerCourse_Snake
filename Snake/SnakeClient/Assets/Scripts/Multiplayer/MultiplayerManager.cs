using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using System;
using Unity.VisualScripting;

public class MultiplayerManager : ColyseusManager<MultiplayerManager> {
    [SerializeField] private PlayerAim _playerAim;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;

    private const string _gameRoomName = "state_handler";
    private ColyseusRoom<State> _room;

    #region Server
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeClient();

        Connection();
    }

    private async void Connection() {
        _room = await client.JoinOrCreate<State>(_gameRoomName);
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
    }

    public void SendMessage(string key, Dictionary<string, object> data) {
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
        snake.Init(player.d);

        PlayerAim playerAim = Instantiate(_playerAim, position, rotation);
        playerAim.Init(snake.Speed);

        Controller controller = Instantiate(_controllerPrefab);
        controller.Init(playerAim, player, snake);
    }
    #endregion

    #region Enemy
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();
    private void CreateEnemy(string key, Player player) {
        Vector3 position = new Vector3(player.x, 0f, player.z);
        Snake snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player.d);

        EnemyController newEnemy = snake.AddComponent<EnemyController>();
        _enemies.Add(key, newEnemy);
        newEnemy.Init(player, snake);
    }

    private void RemoveEnemy(string key, Player value) {
        if (_enemies.ContainsKey(key) == false) Debug.LogError("ѕопытка удалени€ врага, отсутствующегов списке!");
        EnemyController enemy = _enemies[key];
        enemy.Destroy();
        _enemies.Remove(key);
    }
    #endregion
}
