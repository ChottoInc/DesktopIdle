using UnityEngine;

public class UITabQuests : UITabWindow
{
    [SerializeField] TabManager tabManager;

    public override void Open()
    {
        base.Open();

        tabManager.SelectFirstTab();
    }
}
