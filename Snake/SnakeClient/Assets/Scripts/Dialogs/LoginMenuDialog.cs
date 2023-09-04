using CustomEventBus;
using CustomEventBus.Signals;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenuDialog : Dialog {
    [SerializeField] private SkinView _skinViewPrefab;
    [SerializeField] private Transform _skinViewParent;

    [SerializeField] private TMP_InputField _inputLogin;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _returnButton;

    private EventBus _eventBus;
    private PlayerSettings _playerSettings;
    private SkinsManager _skinsManager;

    public void Init() {
        _skinsManager = ServiceLocator.Current.Get<SkinsManager>();
        
        if (!CreateSkinsList()) return;
        
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _playerSettings = ServiceLocator.Current.Get<PlayerSettings>();

        _inputLogin.onValueChanged.AddListener(ValueChangeCheck);
        _startButton.onClick.AddListener(StartGame);
        _returnButton.onClick.AddListener(ReturnToMainMenu);

        _inputLogin.image.color = _skinsManager.Skins.ElementAt(0).HeadMaterial.color;
        _startButton.gameObject.SetActive(false);
    }

    private bool CreateSkinsList()  {
        if (_skinsManager.Skins.Count() == 0) {
            Debug.LogError("SkinsManager: Скины не добавлены в список!");
            return false;
        }

        for (int i = 0; i < _skinsManager.Skins.Count(); i++) {
            SkinData iSkin = _skinsManager.Skins.ElementAt(i);
            SkinView skinView = Instantiate(_skinViewPrefab);
            skinView.transform.SetParent(_skinViewParent, false);
            skinView.Toggle.group = _skinViewParent.GetComponent<ToggleGroup>();

            skinView.Init(i, iSkin.HeadMaterial.color);
            skinView.OnSelectSkin += _skinsManager.SetSkinIndex;
            skinView.OnSelectSkin += SetInputFieldBackgroundColor;
        }
        return true;
    }

    private void SetInputFieldBackgroundColor(int index) {
        _inputLogin.image.color = _skinsManager.Skins.ElementAt(index).HeadMaterial.color;
    }

    public void ValueChangeCheck(string text) {
        if (text.Length == 0) {
            _startButton.gameObject.SetActive(false);
        } else {
            _startButton.gameObject.SetActive(true);
            _playerSettings.SetLogin(text);
        }       
    }

    private void StartGame() {
        _playerSettings.SetSkinId(_skinsManager.CurrentSkinId);
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.Game));
        Hide();
    }

    private void ReturnToMainMenu() {
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.MainMenu));
    }
}

