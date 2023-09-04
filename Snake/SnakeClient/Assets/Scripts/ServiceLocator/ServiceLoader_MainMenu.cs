using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;

/// <summary>
/// Загрузчик сервисов для основного меню
/// </summary>
public class ServiceLoader_MainMenu : ServicesLoader {
    [SerializeField] private GUIHolder _guiHolder;                          // Контейнер UI 
    private MainMenuManager _mainMenuManager;                               // Менеджер основного меню
    
    private void Start() {
        Init();
    }

    public override void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _savesManager = ServiceLocator.Current.Get<SavesManager>();
        _mainMenuManager = new MainMenuManager();
        _mainMenuManager.Init(_eventBus);
        
        RegisterServices();
        AddDisposables();

        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.MainMenu));
    }

    public override void RegisterServices() {
        ServiceLocator.Current.RegisterWithReplacement(_guiHolder);
        ServiceLocator.Current.Register(_mainMenuManager); 

        //Debug.Log("RegisterServices complite");
    }
    
    public override void AddDisposables() {
        _disposables.Add(_mainMenuManager);
    }

    public override void OnDestroy() {
        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }
    }
}

