using CustomEventBus;
using CustomEventBus.Signals;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Dialogs {
    /// <summary>
    /// Окно меню (используется на титульной сцене)
    /// </summary>
    public class MainMenuDialog : Dialog {
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        private EventBus _eventBus;
        public override void Awake() {
            base.Awake();
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            _loginButton.onClick.AddListener(OnLoginButtonClick);
            _settingsButton.onClick.AddListener(OnSettingsClick);
            _quitButton.onClick.AddListener(OnQuitClick);
        }

        private void OnLoginButtonClick() {
            _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.LoginMenu));
        }

        private void OnSettingsClick() {
            _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.Settings));
        }

        private void OnQuitClick() {
            _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.Quit));
        }
    }
}
