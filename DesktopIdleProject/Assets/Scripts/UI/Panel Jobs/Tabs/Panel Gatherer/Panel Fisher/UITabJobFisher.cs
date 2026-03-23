using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITabJobFisher : UITabWindow
{
    [SerializeField] UITabPlayerJob panelJob;

    [Space(10)]
    [SerializeField] GameObject fishGroupPrefab;
    [SerializeField] Transform container;

    private List<GameObject> groupsObjs;

    [Space(10)]
    [SerializeField] GameObject panelFishGroup;
    [SerializeField] GameObject panelLog;
    [SerializeField] TMP_Text textButtonLog;
    [SerializeField] TMP_Text textLog;


    private bool isLogShow;



    private PlayerFisher player;


    private bool isInitialized;


    public override void Open()
    {
        base.Open();
        
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerFisher>();
        }
        
        InitializeIfNeeded();

        panelJob.ChangeCurrentTab(this, UITabPlayerJob.ID_FISHER_TAB);

        // resets
        panelFishGroup.SetActive(true);
        panelLog.SetActive(false);

        textButtonLog.text = "Log";

        isLogShow = false;

        // refresh
        RefreshGroups();
    }

    private void InitializeIfNeeded()
    {
        if (isInitialized) return;

        InitializeGroups();

        isInitialized = true;
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        panelJob.ChangeCurrentTab(null, -1);
    }

    private void InitializeGroups()
    {
        // clear list and refresh groups
        groupsObjs = ClearList(groupsObjs);
        FillGroups();
    }

    private void RefreshGroups()
    {
        foreach (var obj in groupsObjs)
        {
            UIFisherGroupPrefab group = obj.GetComponent<UIFisherGroupPrefab>();
            group.RefreshGroup();
        }
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void FillGroups()
    {
        groupsObjs = new List<GameObject>();

        FishGroupSO[] groups = UtilsGather.GetAllFishGroups();

        for (int i = 0; i < groups.Length; i++)
        {
            GameObject prefab = Instantiate(fishGroupPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UIFisherGroupPrefab obj))
            {
                obj.Setup(groups[i]);
            }
            groupsObjs.Add(prefab);
        }
    }

    public void OnButtonFish()
    {
        if (player != null) return;

        AudioManager.Instance.PlayClickUI();

        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "FisherScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Fisher;

        SceneLoaderManager.Instance.LoadScene(settings);
    }

    public void OnButtonLog()
    {
        AudioManager.Instance.PlayClickUI();

        if (!isLogShow)
        {
            panelFishGroup.SetActive(false);
            panelLog.SetActive(true);

            textButtonLog.text = "Groups";

            FillLog();

            isLogShow = true;
        }
        else
        {
            panelFishGroup.SetActive(true);
            panelLog.SetActive(false);

            textButtonLog.text = "Log";

            isLogShow = false;
        }
    }

    private void FillLog()
    {
        if (FishSpawnManager.Instance == null)
        {
            textLog.text = "";
        }
        else
        {
            string result = "";

            // from last caught to first
            List<FishSO> sessionFishes = new List<FishSO>();
            sessionFishes.AddRange(FishSpawnManager.Instance.CaughtFishesSession);
            sessionFishes.Reverse();

            // fill log with name and rarity of each fish
            foreach (var fish in sessionFishes)
            {
                string singleLine = string.Format(
                "{0}\t" +                       //name
                "<color=#{1}>{2}</color>",      // rarity color and rarity name

                fish.ItemName,
                UtilsItem.GetFishRarityColor(fish.FishRarity),
                UtilsItem.GetFishRarityName(fish.FishRarity)
                );
                result += singleLine + "\n";
            }

            textLog.text = result;
        }
    }
}
