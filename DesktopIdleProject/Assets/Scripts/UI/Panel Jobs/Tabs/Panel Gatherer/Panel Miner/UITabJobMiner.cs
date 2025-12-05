using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabJobMiner : UITabWindow
{
    [SerializeField] UITabJobGatherer panelGatherer;

    [Space(10)]
    [SerializeField] GameObject requirementPrefab;
    [SerializeField] Transform container;

    private List<GameObject> requirementObjs;

    [SerializeField] Button buttonLevelUp;

    private List<ItemGroup> requirements;

    public override void Open()
    {
        base.Open();

        panelGatherer.ChangeCurrentTab(UITabJobGatherer.ID_MINER_TAB);

        // clear list and refill requirements updated to inventory numbers
        requirementObjs = ClearList(requirementObjs);
        FillRequirements();

        // check requirements
        buttonLevelUp.interactable = CheckEnableLevelUp();
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if(list == null)
            list = new List<GameObject>();

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
        return list;
    }

    private void FillRequirements()
    {
        requirementObjs = new List<GameObject>();

        requirements = UtilsGather.GetRequirementsForMinerWeaponLevel(PlayerManager.Instance.PlayerMinerData.WeaponLevel + 1);

        for (int i = 0; i < requirements.Count; i++)
        {
            GameObject prefab = Instantiate(requirementPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(container);

            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.SetActive(true);

            if (prefab.TryGetComponent(out UIMinerWeaponRequirement obj))
            {
                obj.Setup(requirements[i]);
            }
            requirementObjs.Add(prefab);
        }
    }


    /// <summary>
    /// If not all requirements are met returns false
    /// </summary>
    private bool CheckEnableLevelUp()
    {
        foreach (var requirement in requirements)
        {
            if (!PlayerManager.Instance.Inventory.HasEnough(requirement.IdItem, requirement.Quantity))
                return false;
        }
        return true;
    }

    public void OnButtonGather()
    {
        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "MinerScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Miner;

        SceneLoaderManager.Instance.LoadScene(settings);
    }
}
