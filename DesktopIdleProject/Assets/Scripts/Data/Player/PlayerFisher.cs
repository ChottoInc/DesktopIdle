using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFisher : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;


    private PlayerFisherData playerData;


    // ------ FISHING VARS

    public event Action<int, int> OnStatChange;




    public PlayerFisherData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= SaveFisherData;

            playerData.OnStatChange -= OnStatChangeFisher;
        }
    }


    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void Setup(PlayerFisherData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += SaveFisherData;

            playerData.OnStatChange += OnStatChangeFisher;
        }
    }

    public void HandleSwitchScene()
    {
        
    }


    #region SAVE

    public void SaveFisherData()
    {
        PlayerManager.Instance.UpdateFisherData(playerData);
        PlayerManager.Instance.SaveFisherData();
    }

    #endregion

    #region HANDLE EVENTS FROM FISHER DATA

    private void OnStatChangeFisher(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
