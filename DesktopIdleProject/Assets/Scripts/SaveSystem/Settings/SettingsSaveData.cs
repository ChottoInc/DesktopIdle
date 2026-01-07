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

    // ------------ GAMEPLAY

    public bool isAutoBattleOn;
    public bool areTooltipsOn;


    // ------------ VIDEO

    public bool isAlwaysOnTop;
    public bool isClickThrough;
    public bool is60FPS;


    // ------------ AUDIO

    public float masterVolume;






    public SettingsSaveData() { }

    public SettingsSaveData(SettingsManager manager)
    {
        hasSeenIntroTutorial = manager.HasSeenIntroTutorial;



        lastSceneName = manager.LastSceneSettings.lastSceneName;
        lastSceneType = (int)manager.LastSceneSettings.lastSceneType;
        lastCombatMapId = manager.LastSceneSettings.lastCombatMapId;





        isAutoBattleOn = manager.IsAutoBattleOn;
        areTooltipsOn = manager.AreTooltipsOn;



        isAlwaysOnTop = manager.IsAlwaysOnTop;
        isClickThrough = manager.IsClickThrough;
        is60FPS = manager.Is60FPS;



        masterVolume = manager.MasterVolume;
    }
}
