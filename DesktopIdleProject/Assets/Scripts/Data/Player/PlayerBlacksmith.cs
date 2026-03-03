using System;
using UnityEngine;

public class PlayerBlacksmith : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;

    [Header("UI")]
    [SerializeField] GenericBar forgingBar;

    [Header("Animation")]
    [SerializeField] AnimationClip forgeClip;
    [SerializeField] ParticleSystem forgeVFX;


    private PlayerBlacksmithData playerData;

    // ------ ATTACK VARS

    private bool isForging;
    private float CooldownSmash => 1f / playerData.CurrentCraftSpeed;
    private float timerForge;

    // handles forging progress
    private int currentOreId;
    private float currentForgingPoints;


    public int CurrentOreId => currentOreId;
    public bool IsForging => isForging;



    public event Action OnPerformSmash;

    public event Action<int, int> OnStatChange;




    public PlayerBlacksmithData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= SaveBlacksmithData;

            playerData.OnStatChange -= OnStatChangeBlacksmith;
        }

        OnPerformSmash -= CheckForgeProgress;
    }


    private void Awake()
    {
        OnPerformSmash += CheckForgeProgress;
    }

    private void Start()
    {
        OnTryForge();
    }

    private void Update()
    {
        if (isForging)
        {
            CheckForge();
        }
    }

    public void PlayForgeVFX()
    {
        forgeVFX.Play();
    }


    /// <summary>
    /// Check if ore is selected and if enough
    /// </summary>
    private bool CanForge()
    {
        int id = playerData.CurrentForgingOre;

        if(id != -1)
        {
            OreSO ore = UtilsItem.GetItemById(id) as OreSO;

            MetalSO metal = ore.RefinedMetal;

            int needAmount = metal.RequiredOres;

            if(PlayerManager.Instance.Inventory.HasEnough(ore.Id, needAmount))
            {
                currentOreId = id;
                return true;
            }
        }

        return false;
    }

    private void CheckForge()
    {
        if (timerForge <= 0)
        {
            OnPerformSmash?.Invoke();
            timerForge = CooldownSmash;
        }
        else
        {
            timerForge -= Time.deltaTime;
        }
    }

    public void Setup(PlayerBlacksmithData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += SaveBlacksmithData;

            playerData.OnStatChange += OnStatChangeBlacksmith;
        }
    }


    private void CheckForgeProgress()
    {
        // Add progress counter and update UI
        currentForgingPoints += 1f;

        UpdateForgingBarUI();

        // item has been forged
        if(currentForgingPoints >= playerData.CurrentCraftTime)
        {
            SetForging(false);
            
            // Get forging ore
            int id = playerData.CurrentForgingOre;

            OreSO ore = UtilsItem.GetItemById(id) as OreSO;

            // Get refined metal
            MetalSO metal = ore.RefinedMetal;

            // Calculate how much ore to remove
            int needAmount = metal.RequiredOres;

            if(UnityEngine.Random.value <= playerData.CurrentEfficiency)
            {
                // TODO: calculate how much to spare
                // deafult 20% spare materials?

                needAmount -= Mathf.RoundToInt((float)needAmount * 0.2f);
            }

            int amountMetalToAdd = 1;
            if(UnityEngine.Random.value <= playerData.CurrentLuck)
            {
                // if luck procs, dobule material
                amountMetalToAdd *= 2;
            }

            // Update inventory
            PlayerManager.Instance.Inventory.RemoveItem(id, needAmount);

            PlayerManager.Instance.Inventory.AddItem(metal.Id, amountMetalToAdd);

            PlayerManager.Instance.SaveInventoryData();

            // Give exp to blacksmith job
            playerData.AddExp(1); // TODO: check balance
            PlayerManager.Instance.UpdateBlacksmithData(playerData);

            // Recheck for next batch, or idle
            OnTryForge();
        }
    }


    private void SetForgingBarUI()
    {
        forgingBar.SetMaxValue(playerData.CurrentCraftTime);

        currentForgingPoints = 0;
        UpdateForgingBarUI();
    }

    private void UpdateForgingBarUI()
    {
        forgingBar.SetCurrentValue(currentForgingPoints);
    }

    public void SetForging(bool isForging)
    {
        this.isForging = isForging;

        animator.SetBool("isForging", isForging);

        if (isForging)
        {
            forgingBar.gameObject.SetActive(true);
            SetForgingBarUI();
        }
        else
        {
            forgeVFX.Stop();
            forgingBar.gameObject.SetActive(false);
        }
    }

    public void OnTryForge()
    {
        SetForging(CanForge());
    }

    public void HandleSwitchScene()
    {
        SetForging(false);
    }


    public void AddBlacksmithGearLevel(int idGear, int level)
    {
        switch (idGear)
        {
            case UtilsGather.ID_BLACKSMITH_HELMET: playerData.AddBlacksmithHelmetLevel(1); break;
            case UtilsGather.ID_BLACKSMITH_ARMOR: playerData.AddBlacksmithArmorLevel(1); break;
            case UtilsGather.ID_BLACKSMITH_GLOVES: playerData.AddBlacksmithGlovesLevel(1); break;
            case UtilsGather.ID_BLACKSMITH_BOOTS: playerData.AddBlacksmithBootsLevel(1); break;
        }
    }



    #region SAVE

    public void SaveBlacksmithData()
    {
        PlayerManager.Instance.UpdateBlacksmithData(playerData);
        PlayerManager.Instance.SaveMinerData();
    }

    #endregion

    #region HANDLE EVENTS FROM BLACKSMITH DATA

    private void OnStatChangeBlacksmith(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
