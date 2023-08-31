using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelectDialog : MonoBehaviour {
    [SerializeField] private MultiplayerManager _multiplayerManager;

    [SerializeField] private SkinsManager _skinsManager;
    [SerializeField] private SkinView _skinViewPrefab;
    [SerializeField] private Transform _skinViewParent;

    [SerializeField] private TMP_InputField _inputLogin;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    private PlayerSettings _playerSettings;

    private void Awake() {
        if (!CreateSkinsList()) return;
        _playerSettings = PlayerSettings.Instance;

        _inputLogin.onEndEdit.AddListener(InputLogin);
        _startButton.onClick.AddListener(StartGame);
        _exitButton.onClick.AddListener(ExitGame);
    }

    private bool CreateSkinsList() {
        if (_skinsManager.Skins.Count() == 0) {
            Debug.LogError("SkinsManager: Скины не добавлены в список!");
            return false;
        }

        for (int i = 0; i < _skinsManager.Skins.Count(); i++) {
            SkinData iSkin = _skinsManager.Skins.ElementAt(i);
            SkinView skinView = Instantiate(_skinViewPrefab);
            skinView.transform.parent = _skinViewParent;
            skinView.Toggle.group = _skinViewParent.GetComponent<ToggleGroup>();

            skinView.Init(i, iSkin.HeadMaterial.color);
            skinView.OnSelectSkin += _skinsManager.SetSkinIndex;
        }
        return true;
    }

    private void InputLogin(string login) {
        PlayerSettings.Instance.SetLogin(login);
    }

    private void StartGame() {
        _playerSettings.SetSkinId(_skinsManager.CurrentSkinId);
        _multiplayerManager.Connection();
        gameObject.SetActive(false);
    }

    private void ExitGame() {
        Application.Quit();
    }
}
