using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelRecipeIngredients : MonoBehaviour
{
    [Header("Ingredients")]
    [SerializeField] GameObject _requirementPrefab;
    [SerializeField] Transform _container;

    private List<GameObject> _requirementObjs;

    [Header("Crafting settings")]
    [SerializeField] TMP_InputField _inputRefineQuantity;

    private UIInfiniteInputQuantity _customInput;

    private int _selectedCraftedQuantity;


    private RecipeSO _recipeSO;

    private void Awake()
    {
        _customInput = _inputRefineQuantity.GetComponent<UIInfiniteInputQuantity>();
    }

    public void Setup(RecipeSO recipeSO)
    {
        // save recipe
        _recipeSO = recipeSO;

        _requirementObjs = ClearList(_requirementObjs);

        FillRequirements();
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void FillRequirements()
    {
        for (int i = 0; i < _recipeSO.Ingredients.Length; i++)
        {
            GameObject prefab = Instantiate(_requirementPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(_container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UIIngredientPrefab obj))
            {
                obj.Setup(_recipeSO.Ingredients[i]);
            }
            _requirementObjs.Add(prefab);
        }
    }

    private int GetPossibleQuantity(RecipeSO recipe)
    {
        return UtilsAlchemist.GetPossibleQuantity(recipe, PlayerManager.Instance.Inventory);
    }


    public void OnButtonLess()
    {
        AudioManager.Instance.PlayClickUI();

        // if infinite, reduce to maximum, else reduce 1 and check
        if (_selectedCraftedQuantity == -1)
        {
            // get data
            PlayerAlchemistData data = PlayerManager.Instance.PlayerAlchemistData;

            // get ore, if not selected ignore
            RecipeSO recipe = data.CurrentCraftingRecipe;

            if (recipe == null)
            {
                _selectedCraftedQuantity = 0;
                RefreshInputAmountUI();
                return;
            }

            // get quantity
            int maxItem = GetPossibleQuantity(recipe);

            data.SetInfiniteCrafting(false);
            _customInput.SetInfinite(false);

            _selectedCraftedQuantity = maxItem;
            data.SetCurrentCraftingQuantity(_selectedCraftedQuantity);
            _inputRefineQuantity.text = _selectedCraftedQuantity.ToString();

            PlayerManager.Instance.UpdateAlchemistData(data);
            PlayerManager.Instance.SaveAlchemistData();
        }
        else
        {
            _selectedCraftedQuantity--;
            _inputRefineQuantity.text = _selectedCraftedQuantity.ToString();
            OnInputQuantityValueChange(_inputRefineQuantity.text);
        }
    }

    public void OnButtonMore()
    {
        AudioManager.Instance.PlayClickUI();

        // if infinite, nothing, else increase 1 and check
        if (_selectedCraftedQuantity == -1)
        {
            return;
        }
        else
        {
            _selectedCraftedQuantity++;
            _inputRefineQuantity.text = _selectedCraftedQuantity.ToString();
            OnInputQuantityValueChange(_inputRefineQuantity.text);
        }
    }

    public void OnButtonForever()
    {
        AudioManager.Instance.PlayClickUI();

        // get data
        PlayerAlchemistData data = PlayerManager.Instance.PlayerAlchemistData;

        // get ore, if not selected ignore
        RecipeSO recipe = data.CurrentCraftingRecipe;

        if (recipe == null)
        {
            _selectedCraftedQuantity = 0;
            RefreshInputAmountUI();
            return;
        }

        // get quantity
        int maxItem = GetPossibleQuantity(recipe);

        // invert infinite
        if (data.IsInfiniteCrafting)
        {
            data.SetInfiniteCrafting(false);

            _selectedCraftedQuantity = maxItem;
            data.SetCurrentCraftingQuantity(_selectedCraftedQuantity);
            RefreshInputAmountUI();
        }
        else
        {
            data.SetInfiniteCrafting(true);
            _selectedCraftedQuantity = -1;
        }

        _customInput.SetInfinite(data.IsInfiniteCrafting);

        // update data
        PlayerManager.Instance.UpdateAlchemistData(data);
        PlayerManager.Instance.SaveAlchemistData();
    }

    public void OnInputQuantityValueChange(string value)
    {
        // get data
        PlayerAlchemistData data = PlayerManager.Instance.PlayerAlchemistData;

        // get ore, if not selected ignore
        RecipeSO recipe = data.CurrentCraftingRecipe;

        if (recipe == null)
        {
            _selectedCraftedQuantity = 0;
            RefreshInputAmountUI();
            return;
        }

        // get quantity
        int maxItem = GetPossibleQuantity(recipe);

        // set quantity
        if (int.TryParse(value, out int parsed))
        {
            if (parsed < 0)
                parsed = 0;

            if (parsed > maxItem)
                parsed = maxItem;

            // update ui
            _selectedCraftedQuantity = parsed;
            RefreshInputAmountUI();

            // update data
            data.SetCurrentCraftingQuantity(_selectedCraftedQuantity);
            PlayerManager.Instance.UpdateAlchemistData(data);
            PlayerManager.Instance.SaveAlchemistData();
        }
    }

    private void RefreshInputAmountUI()
    {
        _inputRefineQuantity.text = _selectedCraftedQuantity.ToString();
    }
}
