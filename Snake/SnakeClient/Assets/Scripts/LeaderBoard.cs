using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoard : IService, CustomEventBus.IDisposable {
    private int _maxNumberRowsInBoard = 5;
    private LeaderBoardView _leaderBoardView;
    Dictionary<string, PlayerSettings> _leaders = new Dictionary<string, PlayerSettings>();
    private EventBus _eventBus;
    
    public void Init(LeaderBoardView leaderBoardView) {
        _leaderBoardView = leaderBoardView;
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe((AddLeaderSignal signal) => AddLeader(signal));
        _eventBus.Subscribe((RemoveLeaderSignal signal) => RemoveLeader(signal));
    }

    public void AddLeader(AddLeaderSignal signal) {
        string sessionId = signal.Key;
        Player player = signal.Player;

        if (_leaders.ContainsKey(sessionId)) return;

        PlayerSettings newPlayerSettings = new PlayerSettings(player.login, player.sId, player.score);
        _leaders.Add(sessionId, newPlayerSettings);

        UpdateBoard();
    }

    public void RemoveLeader(RemoveLeaderSignal signal) {
        string sessionID = signal.Key;
        if (_leaders.ContainsKey(sessionID) == false) return;
        _leaders.Remove(sessionID);

        UpdateBoard();
    }

    public void UpdateScore(string sessionId, int score) {
        if (_leaders.ContainsKey(sessionId) == false) return;

        _leaders[sessionId].SetScore(score);
        UpdateBoard();
    }

    private void UpdateBoard() {
        int topCount = Mathf.Clamp(_leaders.Count, 0, _maxNumberRowsInBoard);
        IEnumerable<KeyValuePair<string, PlayerSettings>> top = _leaders.OrderByDescending(pair => pair.Value.Score).Take(topCount);
        
        _leaderBoardView.UpdateBoard(top);
    }

    public void Dispose() {
        _eventBus.Unsubscribe((AddLeaderSignal signal) => AddLeader(signal));
        _eventBus.Unsubscribe((RemoveLeaderSignal signal) => RemoveLeader(signal));
    }
}
