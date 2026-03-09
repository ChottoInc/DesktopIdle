using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Farmer/Crop Data", fileName = "CropData_")]
public class CropSO : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] float baseGrowthTime;
    [SerializeField] string cropName;
    [SerializeField] Sprite spriteSeed;
    [SerializeField] Sprite[] spriteCrop;
    [SerializeField] CompanionSO[] attractedCompanions;

    public int Id => id;
    public float BaseGrowthTime => baseGrowthTime;
    public string CropName => cropName;
    public Sprite SpriteSeed => spriteSeed;
    public Sprite[] SpriteCrop => spriteCrop;
    public CompanionSO[] AttractedCompanions => attractedCompanions;
}
