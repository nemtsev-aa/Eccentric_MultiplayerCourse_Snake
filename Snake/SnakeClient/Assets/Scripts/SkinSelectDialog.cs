using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelectDialog : MonoBehaviour {
    [SerializeField] private MultiplayerManager _multiplayerManager;

    [SerializeField] private SkinsManager _skinsManager;
    [SerializeField] private SkinView _skinViewPrefab;
    [SerializeField] private Transform _skinViewParent;

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    private void Awake() {
        if (_skinsManager.Skins.Count() > 0) {
            for (int i = 0; i < _skinsManager.Skins.Count(); i++) {
                SkinData iSkin = _skinsManager.Skins.ElementAt(i);
                SkinView skinView = Instantiate(_skinViewPrefab);
                skinView.transform.parent = _skinViewParent;
                skinView.Toggle.group = _skinViewParent.GetComponent<ToggleGroup>();

                skinView.Init(i, iSkin.HeadMaterial.color);
                skinView.OnSelectSkin += _skinsManager.SetSkinIndex;
            }

            _startButton.onClick.AddListener(StartGame);
            _exitButton.onClick.AddListener(ExitGame);
        } else {
            Debug.LogError("SkinsManager: Скины не добавлены в список!");
        }
    }

    private void StartGame() {
        _multiplayerManager.Connection(_skinsManager.CurrentSkinId);
        gameObject.SetActive(false);
    }

    private void ExitGame() {
        Application.Quit();
    }
}
