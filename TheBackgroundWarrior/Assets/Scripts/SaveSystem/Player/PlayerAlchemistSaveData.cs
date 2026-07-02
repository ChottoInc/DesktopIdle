using System.Collections.Generic;
using System.Linq;

public class PlayerAlchemistSaveData
{
    public int levelStatRoutine;
    public int levelStatYield;
    public int levelStatResearch;
    public int levelStatStability;

    public int availableStatPoints;

    public int currentLevel;
    public long currentExp;

    public int currentCraftingRecipe;
    public bool isInfiniteCrafting;
    public int currentCraftingQuantity;

    public List<int> recipes;

    public PlayerAlchemistSaveData() { }

    public PlayerAlchemistSaveData(PlayerAlchemistData data)
    {
        levelStatRoutine = data.LevelStatRoutine;
        levelStatYield = data.LevelStatYield;
        levelStatResearch = data.LevelStatResearch;
        levelStatStability = data.LevelStatStability;

        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;

        currentCraftingRecipe = data.CurrentCraftingRecipe.Id;
        isInfiniteCrafting = data.IsInfiniteCrafting;
        currentCraftingQuantity = data.CurrentCraftingQuantity;

        recipes = data.AvailableRecipes.Select(recipe => recipe.Id).ToList();
    }
}
