using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlchemist : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;


    [Space(10)]
    [SerializeField] GenericBar barSpell;


    public event Action<int, int> OnStatChange;




    public PlayerAlchemistData PlayerData { get; private set; }



    private void OnDestroy()
    {
        if (PlayerData != null)
        {
            PlayerData.OnLevelUp -= LevelUp;

            PlayerData.OnStatChange -= OnStatChangeMage;
        }
    }

    public void Setup(PlayerAlchemistData playerData)
    {
        PlayerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeMage;
        }
    }

    protected override void Update()
    {
        
    }

    public void ExternalAttack()
    {
        
    }

    private void SpawnSpell()
    {/*
        GameObject spawned = Instantiate(_spellPrefab, transform.position, Quaternion.identity);
        if (spawned.TryGetComponent(out SpellMage spellMage))
        {
            if (spellMage.DoesMove)
            {
                spellMage.SetPositions(castPosition.position, new Vector2(dummyPosition.position.x, castPosition.position.y));
            }
            else
            {
                spellMage.SetPositions(new Vector2(dummyPosition.position.x, castPosition.position.y), new Vector2(dummyPosition.position.x, castPosition.position.y));
            }
            spellMage.Perform();
        }*/
    }

    private void UpdateSpellBar()
    {
        //barSpell.Setup(_currentSpell.RequiredPointsToNextRank, _currentSpell.CurrentLearnPoints);
    }

    public override IBasePlayerData GetPlayerData()
    {
        return PlayerData;
    }

    public override long GetCurrenExp()
    {
        return PlayerData.CurrentExp;
    }

    public override long GetExpToNextLevel()
    {
        return PlayerData.ExpToNextLevel;
    }

    #region SAVE

    public void SaveAlchemistData()
    {
        PlayerManager.Instance.UpdateAlchemistData(PlayerData);
        PlayerManager.Instance.SaveAlchemistData();
    }

    #endregion

    #region HANDLE EVENTS FROM MAGE DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveAlchemistData();
    }

    private void OnStatChangeMage(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
