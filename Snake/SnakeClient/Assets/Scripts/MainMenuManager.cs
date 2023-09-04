using CustomEventBus;
using CustomEventBus.Signals;
using UI.Dialogs;
using UnityEngine;

public class MainMenuManager : IService, IDisposable {
    private MainMenuDialog _mainMenuDialog;
    private EventBus _eventBus;

    public void Init(EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe((MainMenuStateSignal signal) => ShowMainMenu(signal));
    }

    private void ShowMainMenu(MainMenuStateSignal signal) {
        _mainMenuDialog = DialogManager.ShowDialog<MainMenuDialog>();
    }

    public void Dispose() {
        _eventBus.Unsubscribe((MainMenuStateSignal signal) => ShowMainMenu(signal));
    }
}
