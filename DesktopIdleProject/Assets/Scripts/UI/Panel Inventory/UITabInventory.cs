using TMPro;
using UnityEngine;

public class UITabInventory : UITabWindow
{
    public const int ID_INVENTORY_FILTER_ALL = 0;
    public const int ID_INVENTORY_FILTER_ORES = 1;
    public const int ID_INVENTORY_FILTER_METALS = 2;
    public const int ID_INVENTORY_FILTER_CARDS = 20;

    [Header("Currencies")]
    [SerializeField] TMP_Text textBits;

    [Header("Filters")]
    [SerializeField]
    UIInventoryFilterButton[] filterButtons;

    [Header("Window Center")]
    [SerializeField] UIPanelItems panelItems;
    [SerializeField] UIPanelConversion panelConvert;

    [Header("Window Right")]
    [SerializeField] UIPanelConversionList panelConvertList;

    public override void Open()
    {
        base.Open();

        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";

        UpdateFilters();

        panelItems.ShowPanelInfo(false);
        panelItems.Setup(ID_INVENTORY_FILTER_ALL);
    }

    private void UpdateFilters()
    {
        foreach (var filter in filterButtons)
        {
            filter.Refresh();
        }
    }

    public void OpenInventory(int filter)
    {
        panelItems.Setup(filter);

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
        Close();
    }
}
