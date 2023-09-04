using UnityEngine;

public class PlayerSettings : IService {
    public string Login { get; private set; }
    public int SkinId { get; private set; }
    public int Score { get; private set; }

    public PlayerSettings(string login, int skinId, int score) {
        Login = login;
        SkinId = skinId;
        Score = score;
    }

    public PlayerSettings() {
    }

    public void SetLogin(string login) {
        Login = login;
    }
    
    public void SetSkinId(int id) {
        SkinId = id;
    }

    public void SetScore(int score) {
        Score = score;
    }
}
