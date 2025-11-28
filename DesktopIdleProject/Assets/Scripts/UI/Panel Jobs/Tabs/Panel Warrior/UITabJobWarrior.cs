using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITabJobWarrior : UITabWindow
{
    [SerializeField] GameObject mapPrefab;
    [SerializeField] Transform container;

    private CombatMapSO[] maps;
    private List<GameObject> mapObjs;

    public override void Open()
    {
        base.Open();

        if(maps == null)
        {
            maps = UtilsCombatMap.GetAllMaps();
            FillMaps();
        }
        
    }

    private void FillMaps()
    {
        mapObjs = new List<GameObject>();

        for (int i = 0; i < maps.Length; i++)
        {
            GameObject prefab = Instantiate(mapPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            
            if (prefab.TryGetComponent(out UITabWarriorMap obj))
            {
                obj.Setup(this, maps[i]);
            }
            mapObjs.Add(prefab);
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
