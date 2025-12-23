using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabJobMiner : UITabWindow
{
    [SerializeField] UITabPlayerJob panelJob;

    [Space(10)]
    [SerializeField] GameObject requirementPrefab;
    [SerializeField] Transform container;

    private List<GameObject> requirementObjs;

    [Header("Weapon")]
    [SerializeField] Image imageSword;
    [SerializeField] int changeMinerWeaponEvery = 5;

    [Header("Buttons")]
    [SerializeField] Button buttonLevelUp;

    private List<ItemGroup> requirements;


    private PlayerMiner player;


    public override void Open()
    {
        base.Open();

        if(player == null)
        {
            player = FindFirstObjectByType<PlayerMiner>();
        }

        UpdateMinerSwordUI();

        panelJob.ChangeCurrentTab(UITabPlayerJob.ID_MINER_TAB);

        // clear list and refill requirements updated to inventory numbers
        requirementObjs = ClearList(requirementObjs);
        FillRequirements();

        // check requirements
        buttonLevelUp.interactable = CheckEnableLevelUp();
    }

    public void OnButtonBack()
    {
        Close();
        panelJob.ChangeCurrentTab(-1);
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

    /// <summary>
    /// Change sprite of the sword every N levels
    /// </summary>
    private void UpdateMinerSwordUI()
    {
        int indexMinerWeaponSprite = PlayerManager.Instance.PlayerMinerData.WeaponLevel / changeMinerWeaponEvery;

        Sprite sprite = UtilsGather.GetMinerWeaponSpriteByIndex(indexMinerWeaponSprite);
        if(sprite == null)
        {
            sprite = UtilsGather.GetMinerWeaponSpriteByIndex(UtilsGather.GetAllMinerWeaponSprites().Length - 1);
        }

        imageSword.sprite = sprite;
    }



    public void OnButtonGather()
    {
        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "MinerScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Miner;

        SceneLoaderManager.Instance.LoadScene(settings);
    }

    public void OnButtonLevelUp()
    {
        // remove requirements
        // save level and items
        // update sword ui

        foreach (var requirement in requirements)
        {
            PlayerManager.Instance.Inventory.RemoveItem(requirement.IdItem, requirement.Quantity);
        }

        PlayerManager.Instance.SaveInventoryData();

        if(player == null)
        {
            // update directly from save if not in miner scene
            PlayerManager.Instance.PlayerMinerData.AddMinerWeaponLevel(1);
        }
        else
        {
            // or update from temp data if in miner scene, and update from there
            player.AddMinerWeaponLevel(1);
            PlayerManager.Instance.UpdateMinerData(player.PlayerData);
        }

        PlayerManager.Instance.SaveMinerData();

        UpdateMinerSwordUI();
    }
}
