using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabInventory : UITabWindow
{
    public const int ID_INVENTORY_FILTER_ALL = 0;

    [SerializeField] UIPanelItems panelItems;

    public override void Open()
    {
        base.Open();

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
