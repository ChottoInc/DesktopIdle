using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsVideo : UITabWindow
{
    [SerializeField] Toggle toggleAlwaysOnTop;

    [Space(10)]
    [SerializeField] Toggle toggleClickThrough;

    [Space(10)]
    [SerializeField] Toggle toggle30FPS;
    [SerializeField] Toggle toggle60FPS;

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        toggleAlwaysOnTop.SetIsOnWithoutNotify(SettingsManager.Instance.IsAlwaysOnTop);
        toggleClickThrough.SetIsOnWithoutNotify(SettingsManager.Instance.IsClickThrough);

        toggle30FPS.SetIsOnWithoutNotify(!SettingsManager.Instance.Is60FPS);
        toggle60FPS.SetIsOnWithoutNotify(SettingsManager.Instance.Is60FPS);
    }




    public void OnToggleAlwaysOnTop(bool isOn)
    {
        SettingsManager.Instance.SetIsAlwaysOnTop(isOn);
    }

    public void OnToggleClickThrough(bool isOn)
    {
        SettingsManager.Instance.SetIsClickThrough(isOn);
    }



    public void OnToggle30FPS(bool isOn)
    {
        // do something only if isOn, so only one of the group make changes
        if (!isOn) return;

        // set to 30 fps if on
        SettingsManager.Instance.SetIs60FPS(!isOn);
    }

    public void OnToggle60FPS(bool isOn)
    {
        // do something only if isOn, so only one of the group make changes
        if (!isOn) return;

        SettingsManager.Instance.SetIs60FPS(isOn);
    }
}
