using System;
using UnityEngine;

public class PlayerFarmer : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;


    private PlayerFarmerData playerData;


    // ------ FARMER VARS

    public event Action<int, int> OnStatChange;




    public PlayerFarmerData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= SaveFarmerData;

            playerData.OnStatChange -= OnStatChangeFarmer;
        }
    }

    public void Setup(PlayerFarmerData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += SaveFarmerData;

            playerData.OnStatChange += OnStatChangeFarmer;
        }
    }

    public void HandleSwitchScene()
    {

    }


    #region SAVE

    public void SaveFarmerData()
    {
        PlayerManager.Instance.UpdateFarmerData(playerData);
        PlayerManager.Instance.SaveFarmerData();
    }

    #endregion

    #region HANDLE EVENTS FROM FISHER DATA

    private void OnStatChangeFarmer(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
