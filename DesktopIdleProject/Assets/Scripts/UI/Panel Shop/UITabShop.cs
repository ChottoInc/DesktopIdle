using TMPro;
using UnityEngine;

public class UITabShop : UITabWindow
{
    [Header("Currencies")]
    [SerializeField] TMP_Text textBits;

    [Header("Items")]
    [SerializeField] UIPanelShopItems panelShopItems;

    public override void Open()
    {
        base.Open();

        UpdateBitsUI();

        //panelShopItems.ShowPanelInfo(false);
        panelShopItems.Setup(UtilsShop.ID_SHOP_FILTER_CARDPACKS);
    }

    public void OpenInventory(int filter)
    {
        panelShopItems.Setup(filter);
    }


    public void UpdateBitsUI()
    {
        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";
    }


    public override void Close()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        base.Close();
    }

    public void ForceClose()
    {
        base.Close();
    }
}
