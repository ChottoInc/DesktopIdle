using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelSettings : MonoBehaviour
{
    [SerializeField] UITabSettingsGameplay panelGameplay;

    public void Setup()
    {
        //panelGameplay.
    }


    public void OnToggleAutoBattle(bool isOn)
    {
        SettingsManager.Instance.SetIsAutoBattle(isOn);
    }
}
