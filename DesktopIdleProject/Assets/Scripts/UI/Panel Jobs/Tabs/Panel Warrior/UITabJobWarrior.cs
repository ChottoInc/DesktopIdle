using System.Collections.Generic;
using UnityEngine;

public class UITabJobWarrior : UITabWindow
{
    [SerializeField] UITabPlayerJob panelJob;

    [Header("Maps")]
    [SerializeField] GameObject mapPrefab;
    [SerializeField] Transform containerMaps;

    private CombatMapSO[] maps;
    private List<GameObject> mapObjs;

    [Header("Cards")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform containerCards;

    private ItemSO[] cards;
    private List<GameObject> cardObjs;


    public override void Open()
    {
        base.Open();

        panelJob.ChangeCurrentTab(UITabPlayerJob.ID_WARRIOR_TAB);

        if(maps == null)
        {
            maps = UtilsCombatMap.GetAllMaps();
            FillMaps();
        }

        if(cards == null)
        {
            cards = UtilsItem.GetAllCards();
            FillCards();
            RefreshCards();
        }
        else
        {
            RefreshCards();
        }
    }

    public void OnButtonBack()
    {
        Close();
        panelJob.ChangeCurrentTab(-1);
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
                obj.Setup(this, maps[i]);
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
                obj.Setup(this, cardSO);
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
        settings.lastSceneName = mapName + "Scene";
        settings.lastSceneType = SceneLoaderManager.SceneType.CombatMap;
        settings.lastCombatMapId = idMap;

        SceneLoaderManager.Instance.LoadScene(settings);
    }
}
