using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopCardPack : UIShopItem
{
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textPrice;

    private UIPanelShopItems panelShopItems;
    private ShopItemSO itemSO;
    private int currentFilter;

    public override void Setup(UIPanelShopItems panelShopItems, ShopItemSO itemSO, int currentFilter)
    {
        this.panelShopItems = panelShopItems;
        this.itemSO = itemSO;
        this.currentFilter = currentFilter;

        imageItem.sprite = itemSO.Sprite;
        textPrice.text = itemSO.Price.ToString();
    }

    public void OnButtonClick()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        panelShopItems.ShowDetails(itemSO, currentFilter);
    }
}
