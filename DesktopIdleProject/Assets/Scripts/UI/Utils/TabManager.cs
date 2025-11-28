using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] UITab[] tabs;

    private UITab currentTab;

    public void ChangeCurrentTab(UITab selected)
    {
        // Since on deselect is called before opening the new tab, if the tab stops time, it will resume for an instant
        // and stop again when called on select on the next
        // just ensure the tabs all share stops time variable

        if (currentTab != null)
        {
            currentTab.OnDeselect();
        }

        currentTab = selected;

        if (currentTab != null)
        {
            currentTab.OnSelect();
        }
    }
}
