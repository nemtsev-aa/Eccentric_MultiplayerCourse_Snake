using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _placeNumberText;
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _playerScoreText;

    public void Init(PlayerSettings player, int place) {
        SetBackgroundColor(player);
        _placeNumberText.text = $"{place}";
        _playerNameText.text = $"{player.Login}";
        _playerScoreText.text = $"{player.Score}";
    }

    private void SetBackgroundColor(PlayerSettings player) {
        Color snakeColor = ServiceLocator.Current.Get<SkinsManager>().GetSkin(player.SkinId).HeadMaterial.color;
        _background.color = snakeColor;
    }
}
