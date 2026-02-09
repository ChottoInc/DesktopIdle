using UnityEngine;

public abstract class UIShopItem : MonoBehaviour
{
    public abstract void Setup(UIPanelShopItems panelShopItems, ShopItemSO itemSO, int currentFilter);
}
