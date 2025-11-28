using UnityEngine;

public class UITab : MonoBehaviour
{
    [SerializeField] TabManager tabManager;
    [SerializeField] UITabWindow tabWindow;

    [Space(10)]
    [SerializeField] bool stopTime;

    private void Awake()
    {
        tabWindow.OnTabClose += ManualClose;
    }

    private void OnDestroy()
    {
        tabWindow.OnTabClose -= ManualClose;
    }

    public void Select()
    {
        tabManager.ChangeCurrentTab(this);
    }

    public void OnSelect()
    {
        if (stopTime) UtilsTime.Pause();
        tabWindow.Open();
    }

    public void OnDeselect()
    {
        if (stopTime) UtilsTime.Resume();
        tabWindow.Close();
    }

    private void ManualClose()
    {
        if (stopTime) UtilsTime.Resume();
    }
}
