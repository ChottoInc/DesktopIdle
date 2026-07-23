using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelRecipeIngredients : MonoBehaviour
{
    [SerializeField] UITabJobAlchemist _tabAlchemist;

    [Header("Ingredients")]
    [SerializeField] GameObject _requirementPrefab;
    [SerializeField] Transform _container;

    private List<GameObject> _requirementObjs;

    [Header("Crafting settings")]
    [SerializeField] TMP_InputField _inputRefineQuantity;

    private UIInfiniteInputQuantity _customInput;


    private bool _isSameAsCurrent;
    private RecipeSO _tempRecipeSO;
    private int _tempCurrentCraftingQuantity;
    private bool _tempIsInfiniteCrafting;


    private void Awake()
    {
        if(_tabAlchemist != null)
        {
            _tabAlchemist.OnCrafted += UpdateTempToData;
        }

        _customInput = _inputRefineQuantity.GetComponent<UIInfiniteInputQuantity>();
    }

    private void OnDestroy()
    {
        if (_tabAlchemist != null)
        {
            _tabAlchemist.OnCrafted -= UpdateTempToData;
        }
    }

    public void Setup(RecipeSO recipeSO)
    {
        // save recipe
        _tempRecipeSO = recipeSO;

        _requirementObjs = ClearList(_requirementObjs);

        FillRequirements();
        RefreshQuantity();
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
        for (int i = 0; i < _tempRecipeSO.Ingredients.Length; i++)
        {
            GameObject prefab = Instantiate(_requirementPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(_container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UIIngredientPrefab obj))
            {
                obj.Setup(_tempRecipeSO.Ingredients[i]);
            }
            _requirementObjs.Add(prefab);
        }
    }

    private void RefreshQuantity()
    {
        _isSameAsCurrent = false;
        PlayerAlchemistData data = PlayerManager.Instance.PlayerAlchemistData;
        if(data.CurrentCraftingRecipe != null)
        {
            if (_tempRecipeSO.Id == data.CurrentCraftingRecipe.Id)
            {
                _isSameAsCurrent = true;
                _tempCurrentCraftingQuantity = data.CurrentCraftingQuantity;
                _tempIsInfiniteCrafting = _tempCurrentCraftingQuantity == -1 ? true : false;
            }
            else
            {
                _tempCurrentCraftingQuantity = 1;
                _tempIsInfiniteCrafting = false;
            }
        }
        else
        {
            _tempCurrentCraftingQuantity = 1;
            _tempIsInfiniteCrafting = false;
        }

        RefreshInputAmountUI(_tempCurrentCraftingQuantity);
    }

    private int GetPossibleQuantity(RecipeSO recipe)
    {
        return UtilsAlchemist.GetPossibleQuantity(recipe, PlayerManager.Instance.Inventory);
    }


    public void OnButtonLess()
    {
        AudioManager.Instance.PlayClickUI();

        // if infinite, reduce to maximum, else reduce 1 and check
        if (_tempCurrentCraftingQuantity == -1)
        {
            // get quantity
            int maxItem = GetPossibleQuantity(_tempRecipeSO);

            _tempIsInfiniteCrafting = false;
            _customInput.SetInfinite(false);

            _tempCurrentCraftingQuantity = maxItem;
            _inputRefineQuantity.text = _tempCurrentCraftingQuantity.ToString();
        }
        else
        {
            _tempCurrentCraftingQuantity--;
            _inputRefineQuantity.text = _tempCurrentCraftingQuantity.ToString();
            OnInputQuantityValueChange(_inputRefineQuantity.text);
        }

        if (_isSameAsCurrent)
        {
            UpdateTempToData();
        }
    }

    public void OnButtonMore()
    {
        AudioManager.Instance.PlayClickUI();

        // if infinite, nothing, else increase 1 and check
        if (_tempCurrentCraftingQuantity == -1)
        {
            return;
        }
        else
        {
            _tempCurrentCraftingQuantity++;
            _inputRefineQuantity.text = _tempCurrentCraftingQuantity.ToString();
            OnInputQuantityValueChange(_inputRefineQuantity.text);
        }

        if (_isSameAsCurrent)
        {
            UpdateTempToData();
        }
    }

    public void OnButtonForever()
    {
        AudioManager.Instance.PlayClickUI();

        // get quantity
        int maxItem = GetPossibleQuantity(_tempRecipeSO);

        // invert infinite
        if (_tempIsInfiniteCrafting)
        {
            _tempIsInfiniteCrafting = false;

            _tempCurrentCraftingQuantity = maxItem;
            RefreshInputAmountUI(_tempCurrentCraftingQuantity);
        }
        else
        {
            _tempIsInfiniteCrafting = true;
            _tempCurrentCraftingQuantity = -1;
        }

        _customInput.SetInfinite(_tempIsInfiniteCrafting);

        if (_isSameAsCurrent)
        {
            UpdateTempToData();
        }
    }

    public void OnInputQuantityValueChange(string value)
    {
        // get quantity
        int maxItem = GetPossibleQuantity(_tempRecipeSO);

        // set quantity
        if (int.TryParse(value, out int parsed))
        {
            if (parsed < 0)
                parsed = 0;

            if (parsed > maxItem)
                parsed = maxItem;

            // update ui
            _tempCurrentCraftingQuantity = parsed;
            RefreshInputAmountUI(_tempCurrentCraftingQuantity);
        }

        if (_isSameAsCurrent)
        {
            UpdateTempToData();
        }
    }

    private void RefreshInputAmountUI(int quantity)
    {
        _inputRefineQuantity.text = quantity.ToString();
    }

    private void UpdateTempToData()
    {
        PlayerAlchemistData data = PlayerManager.Instance.PlayerAlchemistData;
        data.SetCraftingRecipe(_tempRecipeSO);
        data.SetCurrentCraftingQuantity(_tempCurrentCraftingQuantity);
        data.SetInfiniteCrafting(_tempIsInfiniteCrafting);

        PlayerManager.Instance.UpdateAlchemistData(data);
        PlayerManager.Instance.SaveAlchemistData();
    }
}
