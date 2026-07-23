using System;
using System.Linq;
using UnityEngine;

public class UITabJobAlchemist : UITabWindow
{
    [SerializeField] UITabPlayerJob _panelJob;

    [Header("Recipes")]
    [SerializeField] UIRecipeInfoPrefab[] _recipeInfoPrefabs;

    [Header("Ingredients")]
    [SerializeField] UIPanelRecipeIngredients _panelIngredients;


    private UIRecipeInfoPrefab _lastRecipe;


    public event Action OnCrafted;


    private PlayerAlchemist _player;

   

    public override void Open()
    {
        base.Open();

        if (_player == null)
        {
            _player = FindFirstObjectByType<PlayerAlchemist>();
        }

        // get player
        PlayerAlchemistData data;

        if (_player != null)
        {
            data = _player.PlayerData;
        }
        else
        {
            data = PlayerManager.Instance.PlayerAlchemistData;
        }

        // tell panel jobs which is open
        _panelJob.ChangeCurrentTab(this, UtilsPlayer.PlayerJob.Alchemist);

        // refresh all recipes
        RefreshRecipes();

        SelectSavedRecipe();
    }

    private void SelectSavedRecipe()
    {
        // select saved recipe ui, or first by default
        RecipeSO currentRecipe = PlayerManager.Instance.PlayerAlchemistData.CurrentCraftingRecipe;
        if (currentRecipe != null)
        {
            var uiRecipe = _recipeInfoPrefabs.Where(recipeInfo => recipeInfo.RecipeSO.Id == currentRecipe.Id).First();
            OnSelectRecipe(uiRecipe);
        }
        else
        {
            OnSelectRecipe(_recipeInfoPrefabs[0]);
        }
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        _panelJob.ChangeCurrentTab(null, UtilsPlayer.PlayerJob.None);
    }

    private void RefreshRecipes()
    {
        foreach (var recipe in _recipeInfoPrefabs)
        {
            recipe.Refresh();
        }
    }

    /// <summary>
    /// Shows panel ingredients and set last clicked recipe
    /// </summary>
    /// <param name="uiRecipe">Recipe UI Prefab</param>
    public void OnSelectRecipe(UIRecipeInfoPrefab uiRecipe)
    {
        _lastRecipe = uiRecipe;
        _panelIngredients.Setup(_lastRecipe.RecipeSO);
    }


    public void OnButtonCraft()
    {
        OnCrafted?.Invoke();

        if (_player != null)
        {
            if (!_player.IsCrafting)
            {
                // if it's not already crafting, start
                _player.OnTryCraft();
            }
            else
            {
                // if it's already crafting, but a different item, stop current and start new
                if (_player.CurrentRecipe.Id != _player.PlayerData.CurrentCraftingRecipe.Id)
                {
                    _player.OnTryCraft();
                }
            }
        }
        else
        {
            LastSceneSettings settings = new LastSceneSettings();
            settings.lastSceneName = "AlchemistScene";
            settings.lastSceneType = SceneLoaderManager.SceneType.Alchemist;

            SceneLoaderManager.Instance.LoadScene(settings);
        }
    }
}
