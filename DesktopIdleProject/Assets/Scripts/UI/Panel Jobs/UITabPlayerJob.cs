using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabPlayerJob : UITabWindow
{
    public const int ID_WARRIOR_TAB = 0;
    public const int ID_GATHERER_TAB = 1;

    [SerializeField] UITab tabWarrior;
    [SerializeField] UITab tabGatherer;

    private int currentTab = -1;

    public override void Open()
    {
        base.Open();

        switch (currentTab)
        {
            default:
            case ID_WARRIOR_TAB: tabWarrior.Select(); break;
            case ID_GATHERER_TAB: tabGatherer.Select(); break;
        }
    }

    public void ChangeCurrentTab(int id)
    {
        currentTab = id;
    }

    public void OnButtonClose()
    {
        Close();
    }
}
