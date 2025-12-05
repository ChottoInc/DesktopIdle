using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryPanelInfo : MonoBehaviour
{
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textAmount;
    [SerializeField] TMP_Text textName;

    private ItemGroup group;

    public void Setup(ItemGroup group)
    {
        this.group = group;

        ItemSO item = UtilsItem.GetItemById(group.IdItem);
        imageItem.sprite = item.Sprite;
        textAmount.text = group.Quantity.ToString();
        textName.text = item.ItemName;
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
