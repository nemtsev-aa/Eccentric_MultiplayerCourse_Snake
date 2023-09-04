using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ApplicationState {
    MainMenu,
    LoginMenu,
    Game,
    Pause,
    Settings,
    Quit
}

/// <summary>
/// Менеджер состояний приложения
/// </summary>
public class ApplicationManager : IService {
    private EventBus _eventBus;                                                             // Шина событий
    private ApplicationState _currentApplicationState = ApplicationState.MainMenu;         // Текущее игровое состояние

    public void Init(EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe((ApplicationStateChangedSignal signal) => SetApplicationState(signal));
    }

    public void SetApplicationState(ApplicationState state) {
        _currentApplicationState = state;
    }

    private void SetApplicationState(ApplicationStateChangedSignal signal) {
        _currentApplicationState = signal.CurrentState;
        Debug.Log($"Текущее состояние приложения {_currentApplicationState}");
        
        switch (_currentApplicationState) {
            case ApplicationState.MainMenu:
                _eventBus.Invoke(new MainMenuStateSignal());
                if (SceneManager.GetActiveScene().name != StringConstants.MAINMENU_SCENE_NAME) SceneManager.LoadScene(StringConstants.MAINMENU_SCENE_NAME);
                break;
            case ApplicationState.Game:
                SceneManager.LoadScene(StringConstants.GAME_SCENE_NAME);
                break;
            case ApplicationState.LoginMenu:
                SceneManager.LoadScene(StringConstants.LOGIN_SCENE_NAME);
                break;
            case ApplicationState.Pause:
                _eventBus.Invoke(new PauseStateSignal());
                break;
            case ApplicationState.Settings:
                _eventBus.Invoke(new SettingsStateSignal());
                break;
            case ApplicationState.Quit:
                _eventBus.Invoke(new QuitStateSignal());
                break;
            default:
                break;
        }
    }
}
