using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class ServiceLoader_Game : ServicesLoader {
    [SerializeField] private GUIHolder _guiHolder;                          // Контейнер UI 
    [SerializeField] private LeaderBoard _leaderBoard;                      // Таблица лидеров
    private GameStateManager _gameStateManager;                             // Менеджер приложения в режиме "Игра"

    private void Start() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _leaderBoard = new LeaderBoard();
        _gameStateManager = new GameStateManager();

        RegisterServices();
        Init();
        AddDisposables();
    }

    public override void RegisterServices() {
        ServiceLocator.Current.RegisterWithReplacement(_guiHolder);
        ServiceLocator.Current.Register(_gameStateManager);
        ServiceLocator.Current.Register(_leaderBoard);

        //Debug.Log("RegisterServices complite");
    }

    public override void Init() {
        _gameStateManager.Init(_eventBus);
        _eventBus.Invoke(new GameStartStateSignal());
    }

    public override void AddDisposables() {
        _disposables.Add(_gameStateManager);
    }
}
