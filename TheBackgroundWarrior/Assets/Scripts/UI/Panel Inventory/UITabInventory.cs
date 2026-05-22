using TMPro;
using UnityEngine;

public class UITabInventory : UITabWindow
{
    public const int ID_INVENTORY_FILTER_ALL = 0;
    public const int ID_INVENTORY_FILTER_ORES = 1;
    public const int ID_INVENTORY_FILTER_METALS = 2;
    public const int ID_INVENTORY_FILTER_FISHES = 3;
    public const int ID_INVENTORY_FILTER_CARDS = 20;

    [Header("Currencies")]
    [SerializeField] TMP_Text textBits;

    [Header("Filters")]
    [SerializeField]
    UIInventoryFilterButton[] filterButtons;

    private UIInventoryFilterButton currentFilterButton;
    private int currentFilterId;

    [Header("Window Center")]
    [SerializeField] UIPanelItems panelItems;
    [SerializeField] UIPanelConversion panelConvert;

    [Header("Window Right")]
    [SerializeField] UIPanelConversionList panelConvertList;

    [Header("Texts")]
    [SerializeField] TMP_Text textTitle;
    [SerializeField] TMP_Text textFilterAll;
    [SerializeField] TMP_Text textFilterOres;
    [SerializeField] TMP_Text textFilterMetals;
    [SerializeField] TMP_Text textFilterFishes;
    [SerializeField] TMP_Text textFilterCards;

    public override void Open()
    {
        base.Open();

        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";

        UpdateFilters();

        panelItems.ShowPanelInfo(false);

        currentFilterButton = filterButtons[0];
        currentFilterButton.SelectButton(true);
        panelItems.Setup(ID_INVENTORY_FILTER_ALL);
        currentFilterId = ID_INVENTORY_FILTER_ALL;

        RefreshTexts();
    }

    private void RefreshTexts()
    {
        textTitle.text = UtilsText.AllTextDictionary[UtilsText.text_title_inventory];
        textFilterAll.text = UtilsText.AllTextDictionary[UtilsText.text_button_inventory_filter_all];
        textFilterOres.text = UtilsText.AllTextDictionary[UtilsText.text_button_inventory_filter_ores];
        textFilterMetals.text = UtilsText.AllTextDictionary[UtilsText.text_button_inventory_filter_metals];
        textFilterFishes.text = UtilsText.AllTextDictionary[UtilsText.text_button_inventory_filter_fishes];
        textFilterCards.text = UtilsText.AllTextDictionary[UtilsText.text_button_inventory_filter_cards];
    }

    private void UpdateFilters()
    {
        foreach (var filter in filterButtons)
        {
            filter.Refresh();
        }
    }

    public void RefreshInventory()
    {
        base.Open();

        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";

        UpdateFilters();

        panelItems.ShowPanelInfo(false);

        panelItems.Setup(currentFilterId);
    }

    public void OpenInventory(UIInventoryFilterButton filterButton, int filter)
    {
        // deselect current button filter
        if (currentFilterButton != null)
        {
            currentFilterButton.SelectButton(false);
        }

        if(filterButton != null)
        {
            currentFilterButton = filterButton;
        }

        // select new button filter
        if (currentFilterButton != null)
        {
            currentFilterButton.SelectButton(true);
        }

        panelItems.Setup(filter);
        currentFilterId = filter;

        ClosePanelConvert();
    }


    public void OpenPanelConvert()
    {
        // Hide inventory
        panelItems.ShowPanelInfo(false);
        panelItems.gameObject.SetActive(false);

        // Show Panel Conversion
        panelConvert.Setup();
        panelConvertList.Setup();
    }

    public void ClosePanelConvert()
    {
        // Hide inventory
        panelItems.ShowPanelInfo(false);
        panelItems.gameObject.SetActive(true);

        // Show Panel Conversion
        panelConvert.Close();
    }


    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
    }

    public void OnButtonAddBits()
    {
        if (!SettingsManager.Instance.AreCheatsEnabled) return;

        PlayerManager.Instance.Inventory.AddBits(500);
        PlayerManager.Instance.SaveInventoryData();
        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";
    }
}
