using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;

public class ServiceLoader_Login : ServicesLoader {
    [SerializeField] private GUIHolder _guiHolder;                          // Контейнер UI 
    [SerializeField] private LoginMenuManager _loginMenuManager;            // Менеджер меню входа
    [SerializeField] private MultiplayerManager _multiplayerManager;        
    [SerializeField] private SkinsManager _skinsManager;

    private PlayerSettings _playerSettings;
    private void Start() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _playerSettings = new PlayerSettings();
        _loginMenuManager = new LoginMenuManager();

        RegisterServices();
        Init();
        AddDisposables();
    }

    public override void RegisterServices() {
        ServiceLocator.Current.RegisterWithReplacement(_guiHolder);
        ServiceLocator.Current.Register(_loginMenuManager);
        ServiceLocator.Current.Register(_playerSettings);
        ServiceLocator.Current.Register(_multiplayerManager);
        ServiceLocator.Current.Register(_skinsManager);
        
        //Debug.Log("RegisterServices complite");
    }

    public override void Init() {
        _loginMenuManager.Init(_eventBus);
        _multiplayerManager.Init(_eventBus, _playerSettings, _skinsManager);
        _eventBus.Invoke(new LoginStateSignal());
    }

    public override void AddDisposables() {
        _disposables.Add(_loginMenuManager);
        _disposables.Add(_multiplayerManager);
    }
}
