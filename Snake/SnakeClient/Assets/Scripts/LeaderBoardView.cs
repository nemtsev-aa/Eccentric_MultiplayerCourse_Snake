using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardView : MonoBehaviour {
    [SerializeField] private Transform _playersScoreParent;
    [SerializeField] private PlayerScore _playerScorePrefab;

    public void UpdateBoard(IEnumerable<KeyValuePair<string, PlayerSettings>> leaders) {
        ClearBoardView();
        CreateNewLeaderList(leaders);
    }

    private void ClearBoardView() {
        if (_playersScoreParent.childCount == 0) return;

        int i = 0;
        GameObject[] allChildren = new GameObject[_playersScoreParent.childCount];
        foreach (Transform child in _playersScoreParent) {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        foreach (GameObject child in allChildren) {
            DestroyImmediate(child.gameObject);
        }
    }

    private void CreateNewLeaderList(IEnumerable<KeyValuePair<string, PlayerSettings>> leaders) {
        int i = 1;
        foreach (var item in leaders) {
            PlayerScore playerScore = Instantiate(_playerScorePrefab, _playersScoreParent);
            playerScore.Init(item.Value, i);
            playerScore.transform.SetAsLastSibling();
            i++;
        }
    }
}
