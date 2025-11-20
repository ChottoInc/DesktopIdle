using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private bool isAutoBattleOn;




    public bool IsAutoBattleOn => isAutoBattleOn;



    public static SettingsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    public void SetIsAutoBattle(bool isOn)
    {
        isAutoBattleOn = isOn;
    }
}
