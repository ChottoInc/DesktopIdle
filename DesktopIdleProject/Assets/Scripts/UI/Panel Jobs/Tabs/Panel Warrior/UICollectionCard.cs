using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICollectionCard : MonoBehaviour
{
    [SerializeField] GameObject panelBack;

    [Space(10)]
    [SerializeField] GameObject panelFront;
    [SerializeField] Image imageBackground;
    [SerializeField] Image imageCard;

    [Space(10)]
    [SerializeField] Image imageRarity;
    [SerializeField] TMP_Text textRarity;

    private UITabJobWarrior panelWarrior;

    private CardSO cardSO;
    private bool hasCard;

    public void Setup(UITabJobWarrior panelWarrior, CardSO cardSO)
    {
        this.panelWarrior = panelWarrior;
        this.cardSO = cardSO;

        UpdateCardUI();

        imageRarity.color = UtilsGeneral.GetColorByRarity(cardSO.CardRarity);
        textRarity.text = $"{cardSO.CardRarity}";
    }

    public void Refresh()
    {
        UpdateCardUI();
    }

    private void UpdateCardUI()
    {
        panelFront.SetActive(false);
        panelBack.SetActive(false);

        imageBackground.sprite = cardSO.BackgoundSprite;
        imageCard.sprite = cardSO.Sprite;

        hasCard = PlayerManager.Instance.Inventory.HasItem(cardSO.Id);

        if (hasCard)
        {
            panelFront.SetActive(true);
        }
        else
        {
            panelBack.SetActive(true);
        }
    }

    public void OnPointerEnter()
    {
        if (cardSO != null && hasCard)
        {
            TooltipManagerData tooltipData = new TooltipManagerData();
            tooltipData.idTooltip = UITooltipManager.ID_SHOW_CARD;
            tooltipData.cardSO = cardSO;
            UITooltipManager.Instance.Show(tooltipData, Vector2.zero, true);
        }
    }

    public void OnPointerExit()
    {
        if (!hasCard) return;

        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_CARD, true);
    }
}
