using UnityEngine;

[CreateAssetMenu(fileName = nameof(SkinData), menuName = "ScriptableObjects/Skins/" + nameof(SkinData))]
[System.Serializable]
public class SkinData : ScriptableObject {
    public int ID;
    public Material HeadMaterial;
    public Material DetailMaterial;
    public Material TailMaterial;
}
