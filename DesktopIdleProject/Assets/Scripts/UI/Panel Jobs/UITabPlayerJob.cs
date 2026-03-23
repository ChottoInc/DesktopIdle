using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabPlayerJob : UITabWindow
{
    public const int ID_WARRIOR_TAB = 0;
    public const int ID_MINER_TAB = 1;
    public const int ID_BLACKSMITH_TAB = 2;
    public const int ID_FISHER_TAB = 3;
    public const int ID_FARMER_TAB = 4;

    [SerializeField] ScrollRect panelScroll;
    //[SerializeField] UIButtonJobTab[] jobTabs;

    [Header("Windows")]
    [SerializeField] UITab tabWarrior;
    [SerializeField] UITab tabMiner;
    [SerializeField] UITab tabBlacksmith;
    [SerializeField] UITab tabFisher;
    [SerializeField] UITab tabFarmer;

    private List<UIButtonJobTab> jobTabs;

    private UITabWindow currentTabWindow;
    private int currentTab = -1;

    public override void Open()
    {
        base.Open();

        // Init tabs
        if(jobTabs == null)
        {
            jobTabs = new List<UIButtonJobTab>
            {
                tabWarrior.GetComponent<UIButtonJobTab>(),
                tabMiner.GetComponent<UIButtonJobTab>(),
                tabBlacksmith.GetComponent<UIButtonJobTab>(),
                tabFisher.GetComponent<UIButtonJobTab>(),
                tabFarmer.GetComponent<UIButtonJobTab>()
            };
        }

        // Refresh to check if the job is available now
        foreach (var tab in jobTabs)
        {
            tab.Refresh();
        }

        switch (currentTab)
        {
            default: ResetScrollUI(); break;
            case ID_WARRIOR_TAB: tabWarrior.Select(); break;
            case ID_MINER_TAB: tabMiner.Select(); break;
            case ID_BLACKSMITH_TAB: tabBlacksmith.Select(); break;
            case ID_FISHER_TAB: tabFisher.Select(); break;
            case ID_FARMER_TAB: tabFarmer.Select(); break;
        }
    }

    public void ChangeCurrentTab(UITabWindow window, int id)
    {
        if(id != -1)
        {
            panelScroll.gameObject.SetActive(false);
            currentTabWindow = window;
        }
        else
        {
            currentTabWindow = null;
            ResetScrollUI();
        }

        
        currentTab = id;
    }


    private void ResetScrollUI()
    {
        panelScroll.normalizedPosition = new Vector2(0.5f, 0.5f);
        panelScroll.gameObject.SetActive(true);
    }



    public void OnButtonClose()
    {
        if(currentTabWindow == null)
        {
            AudioManager.Instance.PlayClickUI();
            Close();
        }
        else
        {
            if (currentTabWindow.CanClose())
            {
                AudioManager.Instance.PlayClickUI();
                Close();
            }
        }
    }
}
