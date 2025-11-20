using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] UITab[] tabs;

    private UITab currentTab;

    public void ChangeCurrentTab(UITab selected)
    {
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
