using TMPro;
using UnityEngine;

public class UITabInventory : UITabWindow
{
    public const int ID_INVENTORY_FILTER_ALL = 0;

    [Header("Currencies")]
    [SerializeField] TMP_Text textBits;

    [Header("Items")]
    [SerializeField] UIPanelItems panelItems;

    public override void Open()
    {
        base.Open();

        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";

        panelItems.ShowPanelInfo(false);
        panelItems.Setup(ID_INVENTORY_FILTER_ALL);
    }

    public void OpenInventory(int filter)
    {
        panelItems.Setup(filter);
    }


    public void OnButtonClose()
    {
        Close();
    }
}
