using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabJobBlacksmith : UITabWindow
{
    [SerializeField] UITabPlayerJob panelJob;

    [Space(10)]
    [SerializeField] GameObject requirementPrefab;
    [SerializeField] Transform container;

    private List<GameObject> requirementObjs;

    private int currentGear;

    [Header("Gear")]
    [SerializeField] Image imageGear;
    [SerializeField] TMP_Text textLevel;
    [SerializeField] TMP_Text textStats;

    [Header("Buttons")]
    [SerializeField] UIBlacksmithPanelSelectOre panelSelectOre;

    [Space(10)]
    [SerializeField] Sprite defaultOreIcon;
    [SerializeField] Image imageSelectedOre;
    [SerializeField] Image imageRefinedMetal;

    [Space(10)]
    [SerializeField] Button buttonLevelUp;

    private List<ItemGroup> requirements;


    private PlayerBlacksmith player;


    public override void Open()
    {
        base.Open();

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerBlacksmith>();
        }
        
        UpdateSelectedOreUI();
        UpdateBlacksmithGearUI();

        panelJob.ChangeCurrentTab(this, UITabPlayerJob.ID_BLACKSMITH_TAB);

        // clear list and refill requirements updated to inventory numbers
        requirementObjs = ClearList(requirementObjs);
        FillRequirements(currentGear);

        // check requirements
        buttonLevelUp.interactable = CheckEnableLevelUp();
    }

    public override bool CanClose()
    {
        if (panelSelectOre.IsOpen) return false;

        return true;
    }

    public void OnButtonBack()
    {
        Close();
        panelJob.ChangeCurrentTab(null, -1);
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

    private void FillRequirements(int idGear)
    {
        int gearLevel = 0;

        switch (idGear)
        {
            case UtilsGather.ID_BLACKSMITH_HELMET: gearLevel = PlayerManager.Instance.PlayerBlacksmithData.HelmetLevel + 1; break;
            case UtilsGather.ID_BLACKSMITH_ARMOR: gearLevel = PlayerManager.Instance.PlayerBlacksmithData.ArmorLevel + 1; break;
            case UtilsGather.ID_BLACKSMITH_GLOVES: gearLevel = PlayerManager.Instance.PlayerBlacksmithData.GlovesLevel + 1; break;
            case UtilsGather.ID_BLACKSMITH_BOOTS: gearLevel = PlayerManager.Instance.PlayerBlacksmithData.BootsLevel + 1; break;
        }

        requirements = UtilsGather.GetRequirementsForBlacksmithGearLevel(idGear, gearLevel);

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

    private void UpdateSelectedOreUI()
    {
        PlayerBlacksmithData data;

        if (player != null)
        {
            data = player.PlayerData;
        }
        else
        {
            data = PlayerManager.Instance.PlayerBlacksmithData;
        }

        if (data.CurrentForgingOre != -1)
        {
            OreSO ore = UtilsItem.GetItemById(data.CurrentForgingOre) as OreSO;
            imageSelectedOre.sprite = ore.Sprite;
            imageRefinedMetal.sprite = ore.RefinedMetal.Sprite;
            imageRefinedMetal.gameObject.SetActive(true);
        }
        else
        {
            imageSelectedOre.sprite = defaultOreIcon;
            imageRefinedMetal.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Change sprite of the sword every N levels
    /// </summary>
    private void UpdateBlacksmithGearUI()
    {
        PlayerBlacksmithData data;

        if (player != null)
        {
            data = player.PlayerData;
        }
        else
        {
            data = PlayerManager.Instance.PlayerBlacksmithData;
        }

        int gearLevel = 0;

        switch (currentGear)
        {
            case UtilsGather.ID_BLACKSMITH_HELMET: gearLevel = data.HelmetLevel; break;
            case UtilsGather.ID_BLACKSMITH_ARMOR: gearLevel = data.ArmorLevel; break;
            case UtilsGather.ID_BLACKSMITH_GLOVES: gearLevel = data.GlovesLevel; break;
            case UtilsGather.ID_BLACKSMITH_BOOTS: gearLevel = data.BootsLevel; break;
        }

        int indexBlacksmithGearSprite = gearLevel / UtilsGather.CHANGE_BLACKSMITH_GEARS_EVERY;

        Sprite sprite = UtilsGather.GetBlacksmithGearSpriteByIndex(currentGear, indexBlacksmithGearSprite);
        if (sprite == null)
        {
            sprite = UtilsGather.GetBlacksmithGearSpriteByIndex(currentGear, UtilsGather.GetAllBlacksmithGearSprites(currentGear).Length - 1);
        }

        textLevel.text = $"Lv. {gearLevel}";

        // Multiply by 100 to get percentage, and minus 100 to remove base multiplier

        List<UtilsGeneral.UIStatMultInfo> uiStatsInfos = new List<UtilsGeneral.UIStatMultInfo>();
        switch (currentGear)
        {
            case UtilsGather.ID_BLACKSMITH_HELMET:
                float maxHp = UtilsPlayer.GetBlacksmithHelmetMaxHpMultiplier(data.HelmetLevel);

                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Max Hp", maxHp));
                break;

            case UtilsGather.ID_BLACKSMITH_ARMOR:
                float aDef = UtilsPlayer.GetBlacksmithArmorDefMultiplier(data.ArmorLevel);

                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Def", aDef));
                break;

            case UtilsGather.ID_BLACKSMITH_GLOVES:
                float atkSpd = UtilsPlayer.GetBlacksmithGlovesAtkSpdMultiplier(data.GlovesLevel);
                float critDmg = UtilsPlayer.GetBlacksmithGlovesCritDmgMultiplier(data.GlovesLevel);

                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Atk Speed", atkSpd));
                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Crit Dmg", critDmg));
                break;

            case UtilsGather.ID_BLACKSMITH_BOOTS:
                float bDef = UtilsPlayer.GetBlacksmithBootsDefMultiplier(data.BootsLevel);
                float critRate = UtilsPlayer.GetBlacksmithBootsCritRateMultiplier(data.BootsLevel);

                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Def", bDef));
                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Crit Rate", critRate));
                break;
        }

        string result = string.Empty;
        foreach (var info in uiStatsInfos)
        {
            result += $"{info.statName}: +{(info.multValue * 100f) - 100f}\n";
        }

        textStats.text = result;

        imageGear.sprite = sprite;
    }



    public void OnButtonSelectOre()
    {
        panelSelectOre.Open();
    }

    public void OnSelectedOre(ItemSO oreSO)
    {
        if(player == null)
        {
            PlayerManager.Instance.PlayerBlacksmithData.SetForgingOre(oreSO.Id);
        }
        else
        {
            player.PlayerData.SetForgingOre(oreSO.Id);
            PlayerManager.Instance.UpdateBlacksmithData(player.PlayerData);
        }

        PlayerManager.Instance.SaveBlacksmithData();

        UpdateSelectedOreUI();
    }


    public void OnButtonHelmet()
    {
        currentGear = UtilsGather.ID_BLACKSMITH_HELMET;
        Open();
    }

    public void OnButtonArmor()
    {
        currentGear = UtilsGather.ID_BLACKSMITH_ARMOR;
        Open();
    }

    public void OnButtonGloves()
    {
        currentGear = UtilsGather.ID_BLACKSMITH_GLOVES;
        Open();
    }

    public void OnButtonBoots()
    {
        currentGear = UtilsGather.ID_BLACKSMITH_BOOTS;
        Open();
    }


    public void OnButtonForge()
    {
        if (player != null)
        {
            if (!player.IsForging)
            {
                // if it's not already forging, start
                player.OnTryForge();
            }
            else
            {
                // if it's already forging, but a different item, stop current and start new
                if(player.CurrentOreId != player.PlayerData.CurrentForgingOre)
                {
                    player.OnTryForge();
                }
            }
        }
        else
        {
            LastSceneSettings settings = new LastSceneSettings();
            settings.lastSceneName = "BlacksmithScene";
            settings.lastSceneType = SceneLoaderManager.SceneType.Blacksmith;

            SceneLoaderManager.Instance.LoadScene(settings);
        }
    }

    public void OnButtonLevelUp()
    {
        // remove requirements
        // save level and items
        // update gear ui

        foreach (var requirement in requirements)
        {
            PlayerManager.Instance.Inventory.RemoveItem(requirement.IdItem, requirement.Quantity);
        }

        PlayerManager.Instance.SaveInventoryData();

        if (player == null)
        {
            // update directly from save if not in miner scene
            switch (currentGear)
            {
                case UtilsGather.ID_BLACKSMITH_HELMET: PlayerManager.Instance.PlayerBlacksmithData.AddBlacksmithHelmetLevel(1); break;
                case UtilsGather.ID_BLACKSMITH_ARMOR: PlayerManager.Instance.PlayerBlacksmithData.AddBlacksmithArmorLevel(1); break;
                case UtilsGather.ID_BLACKSMITH_GLOVES: PlayerManager.Instance.PlayerBlacksmithData.AddBlacksmithGlovesLevel(1); break;
                case UtilsGather.ID_BLACKSMITH_BOOTS: PlayerManager.Instance.PlayerBlacksmithData.AddBlacksmithBootsLevel(1); break;
            }
        }
        else
        {
            // or update from temp data if in miner scene, and update from there
            player.AddBlacksmithGearLevel(currentGear, 1);
            PlayerManager.Instance.UpdateBlacksmithData(player.PlayerData);
        }

        PlayerManager.Instance.SaveBlacksmithData();

        UpdateBlacksmithGearUI();
    }
}
