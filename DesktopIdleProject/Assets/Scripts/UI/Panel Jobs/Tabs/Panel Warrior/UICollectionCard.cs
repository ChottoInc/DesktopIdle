using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollectionCard : MonoBehaviour
{
    /*
     * maybe cardso, with reference on how to obtain it for a info panel
     * */

    [SerializeField] Sprite spriteLocked;
    [SerializeField] Image imageCard;

    private UITabJobWarrior panelWarrior;
    private ItemSO card;

    public void Setup(UITabJobWarrior panelWarrior, ItemSO card)
    {
        this.panelWarrior = panelWarrior;
        this.card = card;
    }

    public void Refresh()
    {
        if (PlayerManager.Instance.Inventory.HasItem(card.Id))
        {
            if (card != null)
            {
                imageCard.sprite = card.Sprite;
            }
        }
        else
        {
            imageCard.sprite = spriteLocked;
        }
    }
}
