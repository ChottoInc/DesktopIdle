using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textAmount;

    private UIPanelItems panelItems;
    private ItemGroup group;

    public void Setup(UIPanelItems panelItems, ItemGroup group)
    {
        this.panelItems = panelItems;
        this.group = group;

        imageItem.sprite = UtilsItem.GetItemById(group.IdItem).Sprite;
        textAmount.text = group.Quantity.ToString();
    }

    public void OnItemClick()
    {
        panelItems.ShowDetails(group);
    }
}
