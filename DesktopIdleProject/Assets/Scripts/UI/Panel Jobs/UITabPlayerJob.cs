using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabPlayerJob : UITabWindow
{
    public const int ID_WARRIOR_TAB = 0;

    public const int ID_MINER_TAB = 1;

    [SerializeField] GameObject panelScroll;

    [Header("Windows")]
    [SerializeField] UITab tabWarrior;
    [SerializeField] UITab tabMiner;

    private int currentTab = -1;

    public override void Open()
    {
        base.Open();

        switch (currentTab)
        {
            default: panelScroll.SetActive(true); break;
            case ID_WARRIOR_TAB: tabWarrior.Select(); break;
            case ID_MINER_TAB: tabMiner.Select(); break;
        }
    }

    public void ChangeCurrentTab(int id)
    {
        if(id != -1)
        {
            panelScroll.SetActive(false);
        }
        else
        {
            panelScroll.SetActive(true);
        }

        currentTab = id;
    }

    public void OnButtonClose()
    {
        Close();
    }
}
