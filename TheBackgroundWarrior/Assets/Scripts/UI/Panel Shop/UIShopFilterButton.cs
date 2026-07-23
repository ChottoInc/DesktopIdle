using UnityEngine;
using UnityEngine.UI;

public class UIShopFilterButton : MonoBehaviour
{
    [SerializeField] UITabShop tabShop;
    [SerializeField] int filterId;

    public void OnButtonClick()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        tabShop.OpenShopWindow(this, filterId);
    }
}
