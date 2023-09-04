using CustomEventBus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour {
    private ApplicationManager _applicationController;                  // Менеджер состояний приложения
    private SavesManager _savesManager;                                    // Менеджер сохранений
    private EventBus _eventBus;

    private void Awake() {
        // Инициализация глобальных сервисов
        ServiceLocator.Initialize();
        _eventBus = new EventBus();
        _applicationController = new ApplicationManager();
        _savesManager = new SavesManager();

        RegisterServices();
        Init();
    }

    private void RegisterServices() {
        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_applicationController);
        ServiceLocator.Current.Register(_savesManager);
    }

    private void Init() {
        _applicationController.Init(_eventBus);
        _savesManager.Init();
    }
}