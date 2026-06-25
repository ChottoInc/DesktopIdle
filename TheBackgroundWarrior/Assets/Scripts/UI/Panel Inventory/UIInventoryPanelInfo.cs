using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryPanelInfo : MonoBehaviour
{
    [SerializeField] UITabInventory tabInventory;

    [Space(10)]
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textAmount;
    [SerializeField] TMP_Text textName;

    [Header("Buttons")]
    [SerializeField] GameObject _panelCardButtons;
    [SerializeField] GameObject _panelConsumableButtons;
    [SerializeField] UIPanelDismantle panelDismantle;


    private ItemGroup group;

    private ItemSO itemSO;

    public void Setup(ItemGroup group)
    {
        this.group = group;

        itemSO = UtilsItem.GetItemById(group.IdItem);

        imageItem.sprite = itemSO.Sprite;
        textAmount.text = group.Quantity.ToString();
        textName.text = itemSO.ItemName;

        // Set panel buttons to active if the selected item is a card
        _panelCardButtons.SetActive(itemSO.ItemType == UtilsItem.ItemType.Card);
        _panelConsumableButtons.SetActive(itemSO.ItemType == UtilsItem.ItemType.Bait);
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }



    public void OnButtonConvert()
    {
        tabInventory.OpenPanelConvert();
    }

    public void OnButtonDismantle()
    {
        panelDismantle.Setup(group);
        panelDismantle.Show(true);
        Show(false);
    }



    public void OnButtonUse()
    {
        if (UtilsItem.UseConsumable(itemSO))
        {

        }
    }

}
