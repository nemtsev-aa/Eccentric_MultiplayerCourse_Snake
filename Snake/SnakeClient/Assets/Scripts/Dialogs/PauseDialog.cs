using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PauseDialog : Dialog {
    [Tooltip("Кнопка для продолжения")]
    [SerializeField] private Button _resumeButton;
    [Tooltip("Кнопка для перехода в настройки")]
    [SerializeField] private Button _settingsButton;
    [Tooltip("Кнопка для перехода в меню")]
    [SerializeField] private Button _goToMenuButton;

    private EventBus _eventBus;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _settingsButton.onClick.AddListener(ShowSettings);
        _resumeButton.onClick.AddListener(ResumeGame);
        _goToMenuButton.onClick.AddListener(GoToMenu);

        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
    }

    private void ShowSettings() {
        DialogManager.ShowDialog<SettingsDialog>();
        Hide();
    }

    private void ResumeGame() {
        Hide();
        Time.timeScale = 1f;
        _eventBus.Invoke(new GameResumeSignal());
    }

    private void GoToMenu() {
        Hide();
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.MainMenu));
    }
}
