using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsGameplay : UITabWindow
{
    [Header("Battle")]
    [SerializeField] UIPanelAutoBattle panelAutoBattleSettings;
    [SerializeField] UIPanelAutoBattle panelAutoBattleWorld;

    [Header("HUD")]
    [SerializeField] Toggle toggleInvertedHUD;

    [Header("Floating HUD")]
    [SerializeField] Toggle toggleDamage;
    [SerializeField] Toggle toggleItemCollection;
    [SerializeField] Toggle toggleTooltips;

    [Header("Animations")]
    [SerializeField] Toggle toggleLevelUpEquipmentAnimation;

    [Header("Fisher")]
    [SerializeField] Toggle toggleInvertedFishingSpot;
    [SerializeField] Toggle toggleHideFishingBar;

    [Header("Texts")]
    [SerializeField] TMP_Text textTitleBattle;
    [SerializeField] TMP_Text textTitleHUD;
    [SerializeField] TMP_Text textInvertedHUD;
    [SerializeField] TMP_Text textTitleFloatingHUD;
    [SerializeField] TMP_Text textDamage;
    [SerializeField] TMP_Text textItemCollected;
    [SerializeField] TMP_Text textTooltips;
    [SerializeField] TMP_Text textTitleAnimations;
    [SerializeField] TMP_Text textEquipmentLevelUp;
    [SerializeField] TMP_Text textTitleFisher;
    [SerializeField] TMP_Text textInvertFishingSpot;
    [SerializeField] TMP_Text textHideFishingBar;

    private void Awake()
    {
        panelAutoBattleSettings.OnSet += OnToggleAutoBattleSettings;

        if(panelAutoBattleWorld != null)
            panelAutoBattleWorld.OnSet += OnToggleAutoBattleWorld;
    }

    private void OnDestroy()
    {
        panelAutoBattleSettings.OnSet -= OnToggleAutoBattleSettings;

        if (panelAutoBattleWorld != null)
            panelAutoBattleWorld.OnSet -= OnToggleAutoBattleWorld;
    }

    public override void Open()
    {
        base.Open();

        Setup();

        RefreshTexts();
    }

    private void Setup()
    {
        panelAutoBattleSettings.Setup();


        if (toggleInvertedHUD != null)
            toggleInvertedHUD.SetIsOnWithoutNotify(SettingsManager.Instance.IsInvertedHudOn);


        if (toggleDamage != null)
            toggleDamage.SetIsOnWithoutNotify(SettingsManager.Instance.IsDamageOn);

        if (toggleItemCollection != null)
            toggleItemCollection.SetIsOnWithoutNotify(SettingsManager.Instance.IsItemCollectionOn);

        if (toggleTooltips != null)
            toggleTooltips.SetIsOnWithoutNotify(SettingsManager.Instance.AreTooltipsOn);



        if (toggleLevelUpEquipmentAnimation != null)
            toggleLevelUpEquipmentAnimation.SetIsOnWithoutNotify(SettingsManager.Instance.AreLevelUpEquipmentOn);



        if (toggleInvertedFishingSpot != null)
            toggleInvertedFishingSpot.SetIsOnWithoutNotify(SettingsManager.Instance.IsInvertedFishingSpot);

        if (toggleHideFishingBar != null)
            toggleHideFishingBar.SetIsOnWithoutNotify(SettingsManager.Instance.IsHiddenFishingBar);
    }

    private void RefreshTexts()
    {
        textTitleBattle.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_titlebattle];
        textTitleHUD.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_titlehud];
        textInvertedHUD.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_option_invertedhud];
        textTitleFloatingHUD.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_titlefloatinghud];
        textDamage.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_option_damage];
        textItemCollected.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_option_itemcollected];
        textTooltips.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_option_tooltips];
        textTitleAnimations.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_titleanimations];
        textEquipmentLevelUp.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_option_equipmentlevelup];
        textTitleFisher.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_titlefisher];
        textInvertFishingSpot.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_option_invertfishingspot];
        textHideFishingBar.text = UtilsText.AllTextDictionary[UtilsText.text_settings_gameplay_option_hidefishingbar];
    }


    private void OnToggleAutoBattleSettings(bool isOn)
    {
        if (panelAutoBattleWorld != null)
            panelAutoBattleWorld.SetToggleWithoutNotify(isOn);
    }

    private void OnToggleAutoBattleWorld(bool isOn)
    {
        panelAutoBattleSettings.SetToggleWithoutNotify(isOn);
    }



    public void OnToggleInvertedHUD(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsInvertedHUDOn(isOn);
    }



    public void OnToggleDamage(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsDamageOn(isOn);
    }

    public void OnToggleItemCollection(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsItemCollectionOn(isOn);
    }

    public void OnToggleTooltips(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetAreTooltipsOn(isOn);
    }


    public void OnToggleLevelUpEquipmentAnimation(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetAreLevelUpEquipmentAnimationOn(isOn);
    }


    public void OnToggleInvertedFishingSpot(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsInvertedFishingSpotOn(isOn);
    }

    public void OnToggleHiddenFishingBar(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsHiddenFishingSpot(isOn);
    }
}
