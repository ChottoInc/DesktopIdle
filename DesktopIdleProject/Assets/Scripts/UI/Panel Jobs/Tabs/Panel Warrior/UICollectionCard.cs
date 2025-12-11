using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICollectionCard : MonoBehaviour
{
    [SerializeField] Sprite spriteLocked;
    [SerializeField] Image imageCard;

    [Space(10)]
    [SerializeField] Image imageRarity;
    [SerializeField] TMP_Text textRarity;

    private UITabJobWarrior panelWarrior;
    private CardSO cardSO;

    public void Setup(UITabJobWarrior panelWarrior, CardSO cardSO)
    {
        this.panelWarrior = panelWarrior;
        this.cardSO = cardSO;

        imageRarity.color = UtilsGeneral.GetColorByRarity(cardSO.CardRarity);
        textRarity.text = $"{cardSO.CardRarity}";
    }

    public void Refresh()
    {
        if (PlayerManager.Instance.Inventory.HasItem(cardSO.Id))
        {
            if (cardSO != null)
            {
                imageCard.sprite = cardSO.Sprite;
            }
        }
        else
        {
            imageCard.sprite = spriteLocked;
        }
    }

    public void OnPointerEnter()
    {
        if (cardSO != null)
        {
            TooltipManagerData tooltipData = new TooltipManagerData();
            tooltipData.idTooltip = UITooltipManager.ID_SHOW_CARD;
            tooltipData.cardSO = cardSO;
            UITooltipManager.Instance.Show(tooltipData, Vector2.zero, true);
        }
    }

    public void OnPointerExit()
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_CARD, true);
    }
}
