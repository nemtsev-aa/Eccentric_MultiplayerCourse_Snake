using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

public class GameStateManager : IService, IDisposable {
    private GameStateDialog _gameStateDialog;
    private PauseDialog _pauseDialog;
    private EventBus _eventBus;

    public void Init(EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe((GameStartStateSignal signal) => ShowState(signal));
        _eventBus.Subscribe((PauseStateSignal signal) => ShowPauseMenu(signal));
    }

    private void ShowPauseMenu(PauseStateSignal signal) {
        _pauseDialog = DialogManager.ShowDialog<PauseDialog>();
        _pauseDialog.Init();
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
    }

    private void ShowState(GameStartStateSignal signal) {
        _gameStateDialog = DialogManager.ShowDialog<GameStateDialog>();
        _gameStateDialog.Init();
    }

    public void Dispose() {
        _eventBus.Unsubscribe((GameStartStateSignal signal) => ShowState(signal));
    }
}
