using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDialog : Dialog {
    [SerializeField] private LeaderBoardView _leaderBoardView;
    [SerializeField] private Button _pauseButton;

    private EventBus _eventBus;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _pauseButton.onClick.AddListener(ShowPause);
        ServiceLocator.Current.Get<LeaderBoard>().Init(_leaderBoardView);
    }

    private void ShowPause() {
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.Pause));
    }
}
