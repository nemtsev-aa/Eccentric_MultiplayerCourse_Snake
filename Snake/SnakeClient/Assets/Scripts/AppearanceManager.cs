using System.Collections.Generic;
using UnityEngine;

public enum SnakePart {
    Head,
    Detail,
    Tail
}

public class AppearanceManager : MonoBehaviour {
    [SerializeField] private SnakePart _currentSnakePart;
    [SerializeField] private List<Renderer> _renderers = new List<Renderer>();
    private SkinData _skin;

    public void SetSkin(SkinData skin) {
        _skin = skin;
        Material material = GetMaterialFromSnakePart();
        foreach (Renderer iRenderer in _renderers) {
            iRenderer.material = material;
        }
    }

    public Material GetMaterialFromSnakePart() {
        Material material = null;

        switch (_currentSnakePart) {
            case SnakePart.Head:
                material = _skin.HeadMaterial;
                break;
            case SnakePart.Detail:
                material = _skin.DetailMaterial;
                break;
            case SnakePart.Tail:
                material = _skin.TailMaterial;
                break;
            default:
                break;
        }

        return material;
    }

}
