using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Farmer/Crop Data", fileName = "CropData_")]
public class CropSO : ItemSO
{
    [Space(10)]
    [SerializeField] float baseGrowthTime;

    [Space(10)]
    [SerializeField] int unlocksWithAgronomyLevel;

    [Space(10)]
    [SerializeField] Sprite spriteSeed;
    [SerializeField] Sprite[] spriteCrop;

    [Space(10)]
    [SerializeField] CompanionSO[] attractedCompanions;

    [Space(10)]
    [SerializeField] long rewardedExp;


    public float BaseGrowthTime => baseGrowthTime;

    public int UnlocksWithAgronomyLevel => unlocksWithAgronomyLevel;

    public Sprite SpriteSeed => spriteSeed;
    public Sprite[] SpriteCrop => spriteCrop;

    public CompanionSO[] AttractedCompanions => attractedCompanions;

    public long RewardedExp => rewardedExp; 
}
