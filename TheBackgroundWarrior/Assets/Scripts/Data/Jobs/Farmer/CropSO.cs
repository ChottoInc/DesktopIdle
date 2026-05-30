using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Farmer/Crop Data", fileName = "CropData_")]
public class CropSO : ListableGameDataSO
{
    [Space(10)]
    [SerializeField] float baseGrowthTime;

    [Space(10)]
    [SerializeField] string itemNameTextId;
    [SerializeField] string cropName;

    [Space(10)]
    [SerializeField] Sprite spriteSeed;
    [SerializeField] Sprite[] spriteCrop;

    [Space(10)]
    [SerializeField] CompanionSO[] attractedCompanions;

    [Space(10)]
    [SerializeField] long rewardedExp;

    public float BaseGrowthTime => baseGrowthTime;

    public string CropName
    {
        get
        {
            string res = UtilsText.ItemNamesTextDictionary[itemNameTextId];
            if (res != null) return res; else return cropName;
        }
    }

    public Sprite SpriteSeed => spriteSeed;
    public Sprite[] SpriteCrop => spriteCrop;

    public CompanionSO[] AttractedCompanions => attractedCompanions;

    public long RewardedExp => rewardedExp; 
}
