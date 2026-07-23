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


    public int statPermaMaxHpCounter;
    public int statPermaAttackCounter;
    public int statPermaDefenseCounter; 

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

        if (data.CurrentCraftingRecipe != null)
            currentCraftingRecipe = data.CurrentCraftingRecipe.Id;
        else
            currentCraftingRecipe = -1;

        isInfiniteCrafting = data.IsInfiniteCrafting;
        currentCraftingQuantity = data.CurrentCraftingQuantity;

        recipes = data.AvailableRecipes.Select(recipe => recipe.Id).ToList();


        statPermaMaxHpCounter = data.StatPermaMaxHpCounter;
        statPermaAttackCounter = data.StatPermaAttackCounter;
        statPermaDefenseCounter = data.StatPermaDefenseCounter;
    }
}
