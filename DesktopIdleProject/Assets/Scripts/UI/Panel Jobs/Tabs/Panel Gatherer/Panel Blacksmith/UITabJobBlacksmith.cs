using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabJobBlacksmith : UITabWindow
{
    private int transparencyAmount = Shader.PropertyToID("_Transparency");


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

    [Space(10)]
    [SerializeField] float timerChangeTransparency = 0.75f;
    [SerializeField] float timerChangeTransparencyIdle = 0.5f;

    private int lastWeaponLevel;

    private Material matImageWeapon;

    [Header("Buttons")]
    [SerializeField] UIBlacksmithPanelSelectOre panelSelectOre;

    [Space(10)]
    [SerializeField] TMP_Text textRequired;
    [SerializeField] Sprite defaultOreIcon;
    [SerializeField] Image imageSelectedOre;
    [SerializeField] Image imageRefinedMetal;

    [Space(10)]
    [SerializeField] Button buttonLevelUp;

    [Space(10)]
    [SerializeField] Button buttonHelmet;
    [SerializeField] Button buttonArmor;
    [SerializeField] Button buttonGloves;
    [SerializeField] Button buttonBoots;
    [SerializeField] Color selectedGearColor;

    private Button lastSelectedGearButton;


    private bool isAnimatingLevelUp;


    private List<ItemGroup> requirements;


    private bool isInitialized;


    private PlayerBlacksmith player;


    public override void Open()
    {
        base.Open();

        InitializedIfNeeded();

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerBlacksmith>();
        }

        // Set current gear level at opening
        PlayerBlacksmithData data;

        if (player != null)
        {
            data = player.PlayerData;
        }
        else
        {
            data = PlayerManager.Instance.PlayerBlacksmithData;
        }

        // reset last button gear color
        if(lastSelectedGearButton != null)
        {
            lastSelectedGearButton.image.color = Color.white;
        }

        switch (currentGear)
        {
            case UtilsGather.ID_BLACKSMITH_HELMET: 
                lastWeaponLevel = data.HelmetLevel;
                lastSelectedGearButton = buttonHelmet;
                break;

            case UtilsGather.ID_BLACKSMITH_ARMOR: 
                lastWeaponLevel = data.ArmorLevel;
                lastSelectedGearButton = buttonArmor;
                break;

            case UtilsGather.ID_BLACKSMITH_GLOVES:
                lastWeaponLevel = data.GlovesLevel;
                lastSelectedGearButton = buttonGloves;
                break;

            case UtilsGather.ID_BLACKSMITH_BOOTS: 
                lastWeaponLevel = data.BootsLevel;
                lastSelectedGearButton = buttonBoots;
                break;
        }

        // update selected button color
        lastSelectedGearButton.image.color = selectedGearColor;

        // Update UI
        UpdateSelectedOreUI();
        UpdateBlacksmithGearUI();

        panelJob.ChangeCurrentTab(this, UITabPlayerJob.ID_BLACKSMITH_TAB);

        RefreshRequirements();
    }

    private void InitializedIfNeeded()
    {
        if (isInitialized) return;

        // copy material image ui
        matImageWeapon = new Material(imageGear.material);
        imageGear.material = matImageWeapon;

        isInitialized = true;
    }

    public override bool CanClose()
    {
        if (panelSelectOre.IsOpen) return false;

        return true;
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        panelJob.ChangeCurrentTab(null, -1);
    }

    private void RefreshRequirements()
    {
        // clear list and refill requirements updated to inventory numbers
        requirementObjs = ClearList(requirementObjs);
        FillRequirements(currentGear);

        // check requirements
        buttonLevelUp.interactable = CheckEnableLevelUp();
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

            int groupIndex = PlayerManager.Instance.Inventory.GetGroupIndex(ore.Id);
            ItemGroup oreGroup = PlayerManager.Instance.Inventory.ItemGroups[groupIndex];

            string colorString = "FFFFFF";
            if(oreGroup.Quantity < ore.RefinedMetal.RequiredOres)
            {
                colorString = "878787";
            }

            textRequired.text = string.Format("<color=#{0}>{1}</color> ({2})", colorString, oreGroup.Quantity, ore.RefinedMetal.RequiredOres);
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

        // check for different sprite
        bool isDifferentLevel = lastWeaponLevel != gearLevel;

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
                float maxHp = UtilsBlacksmith.GetBlacksmithHelmetMaxHpMultiplier(data.HelmetLevel);

                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Max Hp", maxHp));
                break;

            case UtilsGather.ID_BLACKSMITH_ARMOR:
                float aDef = UtilsBlacksmith.GetBlacksmithArmorDefMultiplier(data.ArmorLevel);

                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Def", aDef));
                break;

            case UtilsGather.ID_BLACKSMITH_GLOVES:
                float atkSpd = UtilsBlacksmith.GetBlacksmithGlovesAtkSpdMultiplier(data.GlovesLevel);
                float critDmg = UtilsBlacksmith.GetBlacksmithGlovesCritDmgMultiplier(data.GlovesLevel);

                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Atk Speed", atkSpd));
                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Crit Dmg", critDmg));
                break;

            case UtilsGather.ID_BLACKSMITH_BOOTS:
                float bDef = UtilsBlacksmith.GetBlacksmithBootsDefMultiplier(data.BootsLevel);
                float critRate = UtilsBlacksmith.GetBlacksmithBootsCritRateMultiplier(data.BootsLevel);

                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Def", bDef));
                uiStatsInfos.Add(new UtilsGeneral.UIStatMultInfo("Crit Rate", critRate));
                break;
        }

        string result = string.Empty;
        foreach (var info in uiStatsInfos)
        {
            result += $"{info.statName}: +{(info.multValue * 100f) - 100f}%\n";
            //Debug.Log("stat: " + info.statName + ", mult val: " + info.multValue);
        }

        textStats.text = result;

        // Check if need change
        if (!isDifferentLevel)
        {
            imageGear.sprite = sprite;
        }
        else
        {
            // check if Animation is enabled
            if (SettingsManager.Instance.AreLevelUpEquipmentOn)
            {
                StartCoroutine(CoChangeWeaponSprite(sprite));
            }
            else
            {
                imageGear.sprite = sprite;
            }
        }

        lastWeaponLevel = gearLevel;
    }

    private IEnumerator CoChangeWeaponSprite(Sprite newSprite)
    {
        isAnimatingLevelUp = true;

        float elapsedTime = 0;

        float lerpedTransparency = 0;

        // lerp from 0 to 1
        while (elapsedTime < timerChangeTransparency)
        {
            elapsedTime += Time.unscaledDeltaTime;

            lerpedTransparency = Mathf.Clamp01(elapsedTime / timerChangeTransparency);

            matImageWeapon.SetFloat(transparencyAmount, lerpedTransparency);

            yield return null;
        }

        // change sprite
        imageGear.sprite = newSprite;

        // idle
        yield return new WaitForSecondsRealtime(timerChangeTransparencyIdle);


        elapsedTime = 0;

        lerpedTransparency = 1;

        // lerp back from 1 to 0
        while (elapsedTime < timerChangeTransparency)
        {
            elapsedTime += Time.unscaledDeltaTime;

            lerpedTransparency = Mathf.Lerp(1f, 0f, elapsedTime / timerChangeTransparency);

            matImageWeapon.SetFloat(transparencyAmount, lerpedTransparency);

            yield return null;
        }

        isAnimatingLevelUp = false;
    }



    public void OnButtonSelectOre()
    {
        AudioManager.Instance.PlayClickUI();

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
        AudioManager.Instance.PlayClickUI();

        currentGear = UtilsGather.ID_BLACKSMITH_HELMET;
        lastSelectedGearButton = buttonHelmet;
        Open();
    }

    public void OnButtonArmor()
    {
        AudioManager.Instance.PlayClickUI();

        currentGear = UtilsGather.ID_BLACKSMITH_ARMOR;
        lastSelectedGearButton = buttonArmor;
        Open();
    }

    public void OnButtonGloves()
    {
        AudioManager.Instance.PlayClickUI();

        currentGear = UtilsGather.ID_BLACKSMITH_GLOVES;
        lastSelectedGearButton = buttonGloves;
        Open();
    }

    public void OnButtonBoots()
    {
        AudioManager.Instance.PlayClickUI();

        currentGear = UtilsGather.ID_BLACKSMITH_BOOTS;
        lastSelectedGearButton = buttonBoots;
        Open();
    }


    public void OnButtonForge()
    {
        if (player != null)
        {
            if (!player.IsForging)
            {
                // if it's not already forging, start
                AudioManager.Instance.PlayClickUI();
                player.OnTryForge();
            }
            else
            {
                // if it's already forging, but a different item, stop current and start new
                if(player.CurrentOreId != player.PlayerData.CurrentForgingOre)
                {
                    AudioManager.Instance.PlayClickUI();
                    player.OnTryForge();
                }
            }

            panelJob.OnButtonClose();
        }
        else
        {
            AudioManager.Instance.PlayClickUI();

            LastSceneSettings settings = new LastSceneSettings();
            settings.lastSceneName = "BlacksmithScene";
            settings.lastSceneType = SceneLoaderManager.SceneType.Blacksmith;

            SceneLoaderManager.Instance.LoadScene(settings);
        }
    }

    public void OnButtonLevelUp()
    {
        // check if animations are on and animating right now, so you can't interrupt the animation and bug it
        if (SettingsManager.Instance.AreLevelUpEquipmentOn && isAnimatingLevelUp) return;

        AudioManager.Instance.PlayClickUI();

        // remove requirements from inventory
        foreach (var requirement in requirements)
        {
            PlayerManager.Instance.Inventory.RemoveItem(requirement.IdItem, requirement.Quantity);
        }

        // save inventory
        PlayerManager.Instance.SaveInventoryData();

        // add level
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

        // save miner data
        PlayerManager.Instance.SaveBlacksmithData();

        // refresh
        UpdateBlacksmithGearUI();

        // update ui
        RefreshRequirements();
    }
}
