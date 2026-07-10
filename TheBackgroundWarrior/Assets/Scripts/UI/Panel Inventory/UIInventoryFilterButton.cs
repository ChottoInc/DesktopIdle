using UnityEngine;
using UnityEngine.UI;

public class UIInventoryFilterButton : UIBaseTooltipName
{
    [SerializeField] string _textId;

    [Header("Inventory")]
    [SerializeField] UITabInventory tabInventory;
    [SerializeField] UITabInventory.InventoryItemType _filter;
    [SerializeField] UtilsPlayer.PlayerJob[] showIfAvailableJobs;

    [Header("Highlight")]
    [SerializeField] Image imageSelected;
    [SerializeField] Color selectedColor;

    public void Refresh()
    {
        bool canShow = true;

        foreach (var job in showIfAvailableJobs)
        {
            if(!PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(job))
            {
                canShow = false;
                break;
            }
        }

        gameObject.SetActive(canShow);
    }

    public void OnButtonClick()
    {
        tabInventory.OpenInventory(this, _filter);
    }

    public void SelectButton(bool selected)
    {
        if (selected)
            imageSelected.color = selectedColor;
        else
            imageSelected.color = Color.white;
    }

    public override string GetText()
    {
        return UtilsText.AllText[_textId];
    }
}
