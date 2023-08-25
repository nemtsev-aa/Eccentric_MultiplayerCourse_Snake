using System;
using UnityEngine;
using UnityEngine.UI;

public class SkinView : MonoBehaviour {
    public Toggle Toggle => _toggle;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _bodyImage;

    public Action<int> OnSelectSkin;
    private int _skinIndex;

    public void Init(int skinIndex, Color color) {
        _skinIndex = skinIndex;
        _bodyImage.color = color;
        _toggle.onValueChanged.AddListener(SelectSkin);
    }

    private void SelectSkin(bool value) {
        if (value) OnSelectSkin?.Invoke(_skinIndex);
    }
}
