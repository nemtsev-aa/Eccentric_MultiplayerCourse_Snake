using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour {
    public int CurrentSkinId => _currentSkinId;
    public IEnumerable<SkinData> Skins => _skins;
    [SerializeField] private List<SkinData> _skins = new List<SkinData>();
    [SerializeField] private int _currentSkinId;

    public SkinData GetSkin(int index) {
        if (_skins.Count == 0) {
            Debug.LogError("SkinsManager: Скины не добавлены в список!");
            return null;
        } else {
            SkinData skin = _skins[index];
            return skin;
        }
    }

    public void SetSkinIndex(int index) {
        if (index <= _skins.Count-1) {
            _currentSkinId = index;
        } 
    }
}
