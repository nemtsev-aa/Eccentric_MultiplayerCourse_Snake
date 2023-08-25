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

    public void SetSkin(SkinData skin) {
        Material material = null;

        switch (_currentSnakePart) {
            case SnakePart.Head:
                material = skin.HeadMaterial;
                break;
            case SnakePart.Detail:
                material = skin.DetailMaterial;
                break;
            case SnakePart.Tail:
                material = skin.TailMaterial;
                break;
            default:
                break;
        }

        foreach (Renderer iRenderer in _renderers) {
            iRenderer.material = material;
        }
    }
}
