using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSaveData
{
    // ---- TUTORIAL ----

    public bool hasSeenIntroTutorial;


    // ---- LAST SCENE ----

    public string lastSceneName;

    public int lastSceneType;

    // combat map
    public int lastCombatMapId;


    // ---- SETTINGS ----


    public bool isAutoBattleOn;

    public SettingsSaveData() { }

    public SettingsSaveData(SettingsManager manager)
    {
        hasSeenIntroTutorial = manager.HasSeenIntroTutorial;



        lastSceneName = manager.LastSceneSettings.lastSceneName;
        lastSceneType = (int)manager.LastSceneSettings.lastSceneType;
        lastCombatMapId = manager.LastSceneSettings.lastCombatMapId;



        isAutoBattleOn = manager.IsAutoBattleOn;
    }
}
