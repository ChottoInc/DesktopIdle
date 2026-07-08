using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Alchemist/Recipe Data", fileName = "RecipeData_")]
public class RecipeSO : ListableGameDataSO
{
    [SerializeField] GenericRequirement[] _ingredients;
    [SerializeField] ItemSO _product;
    [SerializeField] float _craftTime;

    [Space(10)]
    [SerializeField] long _rewardedExp;

    public GenericRequirement[] Ingredients => _ingredients;
    public ItemSO Product => _product;
    public float CraftTime => _craftTime;

    public long RewardedExp => _rewardedExp;
}
