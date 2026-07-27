using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayerDataProvider
{
    // every sub class of player need a list of buffs to check internally
    protected List<UtilsBuffs.BuffType> _buffsToCheckTypes;
    protected float timer1Second = UtilsGeneral.TIMER_1SECONDS;


    public event Action<ItemSO> OnItemAdd;

    public event Action OnLevelUp;


    protected virtual void Awake()
    {
        PlayerManager.Instance.PlayerBuffsData.OnAddBuff += OnBuffAdded;
        PlayerManager.Instance.PlayerBuffsData.OnRemoveBuff += OnBuffRemoved;
    }

    protected virtual void OnDestroy()
    {
        PlayerManager.Instance.PlayerBuffsData.OnAddBuff -= OnBuffAdded;
        PlayerManager.Instance.PlayerBuffsData.OnRemoveBuff -= OnBuffRemoved;
    }


    protected virtual void Update()
    {
        if(_buffsToCheckTypes != null)
            HandleBuffs();
    }

    protected virtual void HandleBuffs()
    {
        if (timer1Second <= 0)
        {
            PlayerManager.Instance.PlayerBuffsData.DecreaseBuffs(_buffsToCheckTypes, 1f);

            timer1Second = UtilsGeneral.TIMER_1SECONDS;

            PlayerManager.Instance.SaveBuffsData();
        }
        else
        {
            timer1Second -= Time.deltaTime;
        }
    }

    protected virtual void OnBuffAdded(UtilsBuffs.BuffType buffType)
    {
        //Debug.Log("Need to inherit from player");
    }

    protected virtual void OnBuffRemoved(UtilsBuffs.BuffType buffType)
    {
        //Debug.Log("Need to inherit from player");
    }

    
    public virtual void AddItem(int id, int quantity)
    {
        PlayerManager.Instance.Inventory.AddItem(id, quantity);
        PlayerManager.Instance.SaveInventoryData();

        ItemSO itemSO = UtilsItem.GetItemById(id);
        AddItemEvent(itemSO);
    }

    public virtual void AddItemEvent(ItemSO itemSO)
    {
        OnItemAdd?.Invoke(itemSO);
    }

    protected virtual void LevelUp()
    {
        OnLevelUp?.Invoke();
    }

    /// <summary>
    /// Get the player data interface
    /// </summary>
    public virtual IBasePlayerData GetPlayerData()
    {
        throw new NotImplementedException("Class needs to override get player data function");
    }

    public virtual long GetCurrenExp()
    {
        throw new NotImplementedException("Class needs to override get current exp function");
    }

    public virtual long GetExpToNextLevel()
    {
        throw new NotImplementedException("Class needs to override get exp next level function");
    }
}
