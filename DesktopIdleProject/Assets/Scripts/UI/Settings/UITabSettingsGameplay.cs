using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsGameplay : UITabWindow
{
    [Header("Battle")]
    [SerializeField] UIPanelAutoBattle panelAutoBattleSettings;
    [SerializeField] UIPanelAutoBattle panelAutoBattleWorld;

    [Header("Floating HUD")]
    [SerializeField] Toggle toggleDamage;
    [SerializeField] Toggle toggleItemCollection;
    [SerializeField] Toggle toggleTooltips;

    [Header("Animations")]
    [SerializeField] Toggle toggleLevelUpEquipmentAnimation;

    private void Awake()
    {
        panelAutoBattleSettings.OnSet += OnToggleAutoBattleSettings;
        panelAutoBattleWorld.OnSet += OnToggleAutoBattleWorld;
    }

    private void OnDestroy()
    {
        panelAutoBattleSettings.OnSet -= OnToggleAutoBattleSettings;
        panelAutoBattleWorld.OnSet -= OnToggleAutoBattleWorld;
    }

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        panelAutoBattleSettings.Setup();

        if (toggleTooltips != null)
            toggleTooltips.SetIsOnWithoutNotify(SettingsManager.Instance.AreTooltipsOn);
        else
            Debug.Log("Settings panel does not have auto battle world panel linked");
    }


    private void OnToggleAutoBattleSettings(bool isOn)
    {
        panelAutoBattleWorld.SetToggleWithoutNotify(isOn);
    }

    private void OnToggleAutoBattleWorld(bool isOn)
    {
        panelAutoBattleSettings.SetToggleWithoutNotify(isOn);
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
}
