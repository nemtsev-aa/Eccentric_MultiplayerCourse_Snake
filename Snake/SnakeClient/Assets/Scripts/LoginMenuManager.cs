using CustomEventBus;
using CustomEventBus.Signals;

public class LoginMenuManager : IService, IDisposable {
    private LoginMenuDialog _loginMenuDialog;
    private EventBus _eventBus;

    public void Init(EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe((LoginStateSignal signal) => ShowMenu(signal));
    }

    private void ShowMenu(LoginStateSignal signal) {
        _loginMenuDialog = DialogManager.ShowDialog<LoginMenuDialog>();
        _loginMenuDialog.Init();
    }

    public void Dispose() {
        _eventBus.Unsubscribe((LoginStateSignal signal) => ShowMenu(signal));
    }
}
