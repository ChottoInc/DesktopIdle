using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsAudio : UITabWindow
{
    [SerializeField] Slider sliderMaster;

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        sliderMaster.SetValueWithoutNotify(SettingsManager.Instance.MasterVolume);
    }




    public void OnMasterChange(float value)
    {
        SettingsManager.Instance.SetMasterVolume(value);
    }
}
