using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Alchemist/Research Item Data", fileName = "ResearchItemData_")]
public class ResearchItemSO : ListableGameDataSO
{
    [SerializeField] RecipeSO _unlocksRecipe;

    public RecipeSO UnlocksRecipe => _unlocksRecipe;
}
