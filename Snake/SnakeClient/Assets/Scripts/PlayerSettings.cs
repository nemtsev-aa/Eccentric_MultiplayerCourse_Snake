using UnityEngine;

public class PlayerSettings : MonoBehaviour {
    public static PlayerSettings Instance;
    public string Login { get; private set; }
    public int SkinId { get; private set; }
    
    private void Awake() {

        if (Instance) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy() {
        if (Instance == this) Instance = null;
    }

    public void SetLogin(string login) {
        Login = login;
    }
    
    public void SetSkinId(int id) {
        SkinId = id;
    }
}
