using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomSteamManager : SteamManager
{
    //private event Action OnInitialize;

    private bool checkedSettings;

    protected override void Awake()
    {
        if (!SettingsManager.Instance.IsSteamPlatform)
        {
            Destroy(gameObject);
            return;
        }

        base.Awake();

        if (Initialized)
        {
            //OnInitialize?.Invoke();
            OnInitialize();
        }
    }

    protected override void OnDestroy()
    {
        if (!SettingsManager.Instance.IsSteamPlatform) return;

        base.OnDestroy();
    }

    protected override void OnEnable()
    {
        if (!SettingsManager.Instance.IsSteamPlatform) return;

        base.OnEnable();
    }

    protected override void Update()
    {
        if (!SettingsManager.Instance.IsSteamPlatform) return;

        // once the initializer manager is init check if game has client language
        if (!checkedSettings && InitializerManager.Instance.HasCheckFiles)
        {
            // only check once
            checkedSettings = true;

            // if game language is the default one means the first open or the player set it to english, so check if game has client language
            if(SettingsManager.Instance.CurrentLanguage == UtilsGeneral.Language.Eng)
            {
                SettingsManager.Instance.CheckSteamLanguage(SteamApps.GetCurrentGameLanguage());
            }

        }

        base.Update();
    }

    private void OnInitialize()
    {
        
    }

    protected virtual string GetUserInfo()
    {
        string id = SteamUser.GetSteamID().ToString();
        string lev = SteamUser.GetPlayerSteamLevel().ToString();

        return string.Format("name: {0}, lv: {1}", id, lev);
    }
}
