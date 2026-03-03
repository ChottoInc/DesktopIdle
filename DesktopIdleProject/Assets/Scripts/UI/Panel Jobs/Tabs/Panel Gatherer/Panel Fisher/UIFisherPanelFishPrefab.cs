using UnityEngine;
using UnityEngine.UI;

public class UIFisherPanelFishPrefab : MonoBehaviour
{
    [SerializeField] Image imageFish;
    [SerializeField] Color lockedColor;
    [SerializeField] Color unlockedColor;
    [SerializeField] Transform tooltipPosition;

    private FishSO fishSO;

    public void Setup(FishSO fishSO)
    {
        this.fishSO = fishSO;

        imageFish.sprite = fishSO.Sprite;

        if (PlayerManager.Instance.Inventory.HasItem(fishSO.Id))
        {
            imageFish.color = unlockedColor;
        }
        else
        {
            imageFish.color = lockedColor;
        }
    }

    public void OnPointerEnter()
    {
        string itemName = "N/A";
        if (fishSO != null)
        {
            itemName = string.Format(
                "{0}\n" +                       //name
                "<color=#{1}>{2}</color>\n" +   // rarity color and rarity name
                "Spawn time: {3}",              // spawn moment in day
                
                fishSO.ItemName,
                UtilsItem.GetFishRarityColor(fishSO.FishRarity),
                UtilsItem.GetFishRarityName(fishSO.FishRarity),
                UtilsGeneral.GetDayMomentName(fishSO.SpawnDayMoment));
        }

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = itemName;
        UITooltipManager.Instance.Show(tooltipData, tooltipPosition.position, true);
    }

    public void OnPointerExit()
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }
}
