using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using System;

public class MultiplayerManager : ColyseusManager<MultiplayerManager> {
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
        Snake snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player.d);

        Controller controller = Instantiate(_controllerPrefab);
        controller.Init(snake);
    }
    #endregion

    #region Enemy
    private void CreateEnemy(string key, Player player) {
        
    }

    private void RemoveEnemy(string key, Player value) {
        
    }
    #endregion
}
