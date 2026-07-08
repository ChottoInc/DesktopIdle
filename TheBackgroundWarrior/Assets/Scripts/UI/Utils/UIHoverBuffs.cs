using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverBuffs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_BUFFS;
        UITooltipManager.Instance.Show(tooltipData, transform.position, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_BUFFS, true);
    }
}
