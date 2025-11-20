using UnityEngine;

public class UITab : MonoBehaviour
{
    [SerializeField] TabManager tabManager;
    [SerializeField] UITabWindow tabWindow;

    public void Select()
    {
        tabManager.ChangeCurrentTab(this);
    }

    public void OnSelect()
    {
        tabWindow.Open();
    }

    public void OnDeselect()
    {
        tabWindow.Close();
    }
}
