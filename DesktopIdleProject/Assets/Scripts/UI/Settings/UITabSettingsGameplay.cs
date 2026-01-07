using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsGameplay : UITabWindow
{
    [Header("Auto-battle")]
    [SerializeField] UIPanelAutoBattle panelAutoBattleSettings;
    [SerializeField] UIPanelAutoBattle panelAutoBattleWorld;

    [Header("Tooltips")]
    [SerializeField] Toggle toggleTooltips;

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




    public void OnToggleTooltips(bool isOn)
    {
        SettingsManager.Instance.SetAreTooltipsOn(isOn);
    }
}
