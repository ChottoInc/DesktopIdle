using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSound : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        AudioManager.Instance.PlayClickUI();
    }
}
