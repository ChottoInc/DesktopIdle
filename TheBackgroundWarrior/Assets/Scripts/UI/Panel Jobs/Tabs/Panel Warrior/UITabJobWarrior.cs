using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabJobWarrior : UITabWindow
{
    [SerializeField] UITabPlayerJob panelJob;

    [Header("Maps")]
    [SerializeField] GameObject mapPrefab;
    [SerializeField] Transform containerMaps;
    [SerializeField] ScrollRect scrollMaps;

    private CombatMapSO[] maps;
    private List<GameObject> mapObjs;

    [Header("Cards")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform containerCards;
    [SerializeField] ScrollRect scrollCards;

    private ItemSO[] cards;
    private List<GameObject> cardObjs;

    [Header("Texts")]
    [SerializeField] TMP_Text textTitle;
    [SerializeField] TMP_Text textTitleMaps;
    [SerializeField] TMP_Text textJobDescription;

    private PlayerFight player;


    public override void Open()
    {
        base.Open();

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerFight>();
        }

        panelJob.ChangeCurrentTab(this, UITabPlayerJob.ID_WARRIOR_TAB);

        if(maps == null)
        {
            maps = UtilsCombatMap.GetAllMaps();
            FillMaps();

            // set map scroll to point near your last unlocked map
            int mapsCount = PlayerManager.Instance.PlayerFightData.AvailableMaps.Count;
            if (mapsCount > 3)
            {
                scrollMaps.verticalNormalizedPosition = 1f / (float)mapsCount;
            }
        }

        if(cards == null)
        {
            cards = UtilsItem.GetAllTypeItem<CardSO>();
            FillCards();
            RefreshCards();
        }
        else
        {
            RefreshCards();
        }

        RefreshTexts();
    }

    private void RefreshTexts()
    {
        textTitle.text = UtilsText.AllTextDictionary[UtilsText.text_title_jobs_back];
        textTitleMaps.text = UtilsText.AllTextDictionary[UtilsText.text_title_jobs_warrior_maps];
        textJobDescription.text = UtilsText.HelpTextDictionary[UtilsText.text_description_warrior];
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        panelJob.ChangeCurrentTab(null, -1);
    }

    private void FillMaps()
    {
        mapObjs = new List<GameObject>();

        for (int i = 0; i < maps.Length; i++)
        {
            GameObject prefab = Instantiate(mapPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(containerMaps);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            
            if (prefab.TryGetComponent(out UITabWarriorMap obj))
            {
                obj.Setup(this, maps[i], scrollMaps);
            }
            mapObjs.Add(prefab);
        }
    }

    private void FillCards()
    {
        cardObjs = new List<GameObject>();

        for (int i = 0; i < cards.Length; i++)
        {
            GameObject prefab = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(containerCards);

            prefab.transform.localScale = new Vector3(1, 1, 1);

            if (prefab.TryGetComponent(out UICollectionCard obj))
            {
                CardSO cardSO = cards[i] as CardSO;
                obj.Setup(this, cardSO, scrollCards);
            }
            cardObjs.Add(prefab);
        }
    }

    private void RefreshCards()
    {
        foreach (var card in cardObjs)
        {
            if(card.TryGetComponent(out UICollectionCard obj))
            {
                obj.Refresh();
            }
        }
    }






    public void OnMapSelected(string mapName, int idMap)
    {
        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = mapName;
        settings.lastSceneType = SceneLoaderManager.SceneType.CombatMap;
        settings.lastCombatMapId = idMap;

        SceneLoaderManager.Instance.LoadScene(settings);


        /*
        if(player != null)
        {
            // if already in the scene do not load
            if(SettingsManager.Instance.LastSceneSettings.lastSceneName == mapName)
            {

            }
        }
        else
        {
            // 100% not in a fight scene
            LastSceneSettings settings = new LastSceneSettings();
            settings.lastSceneName = mapName;
            settings.lastSceneType = SceneLoaderManager.SceneType.CombatMap;
            settings.lastCombatMapId = idMap;

            SceneLoaderManager.Instance.LoadScene(settings);
        }
        */
    }
}
