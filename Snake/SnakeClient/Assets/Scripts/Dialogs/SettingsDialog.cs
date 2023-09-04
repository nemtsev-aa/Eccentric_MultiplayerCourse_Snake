using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDialog : Dialog {
    [Tooltip(" нопка дл€ продолжени€")]
    [SerializeField] private Button _resumeButton;
    [Tooltip(" нопка дл€ перехода в меню")]
    [SerializeField] private Button _goToMenuButton;

    private EventBus _eventBus;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _resumeButton.onClick.AddListener(ResumeTask);
        _goToMenuButton.onClick.AddListener(GoToMenu);
    }

    private void ResumeTask() {
        Hide();
        _eventBus.Invoke(new GameResumeSignal());
    }

    private void GoToMenu() {
        Hide();
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.MainMenu));
    }
}
