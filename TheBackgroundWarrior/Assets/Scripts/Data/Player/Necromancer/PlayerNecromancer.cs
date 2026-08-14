using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNecromancer : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;


    [Space(10)]
    [SerializeField] Transform[] _fightPositions;

    private GameObject _spellPrefab;


    public event Action<int, int> OnStatChange;




    public PlayerNecromancerData PlayerData { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        //OnStatChange += CheckSpellBar;
    }

    private void Start()
    {
        _buffsToCheckTypes = new List<UtilsBuffs.BuffType>()
        {
            UtilsBuffs.BuffType.Greed,
            UtilsBuffs.BuffType.Veteran,
            //UtilsBuffs.BuffType.Arcanist,
        };
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        //OnStatChange -= CheckSpellBar;

        if (PlayerData != null)
        {
            PlayerData.OnLevelUp -= LevelUp;

            PlayerData.OnStatChange -= OnStatChangeNecromancer;
        }
    }

    public void Setup(PlayerNecromancerData playerData)
    {
        PlayerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeNecromancer;

            //RefreshSpell();
        }
    }
    /*
    public void RefreshSpell()
    {
        if (PlayerData.CurrentLearningSpell != UtilsMage.MageSpellType.None)
        {
            // get spell and assign prefab to ast
            _currentSpell = PlayerData.GetSpellByType(PlayerData.CurrentLearningSpell);
            _spellPrefab = _currentSpell.SpellSO.Prefab;

            // update learning points ui
            UpdateSpellBar();

            // update timers
            UpdateCooldownCast();
            _timerCast = _finalCooldownCast;

            _barCooldown.Setup(_finalCooldownCast, 0f);

        }
    }
    */
    protected override void Update()
    {
        base.Update();
    }

    private void CastSpell()
    {
        /*
        // animator
        animator.SetTrigger("Attack");

        // add points and update player data
        int finalPointsToAdd = 1;

        // check if player has arcanist buff
        if (PlayerManager.Instance.PlayerBuffsData.HasBuff(UtilsBuffs.BuffType.Arcanist))
        {
            finalPointsToAdd *= 2;
        }
        _currentSpell.AddPoints(finalPointsToAdd);

        PlayerData.UpdateSpellData(_currentSpell);
        PlayerData.AddExp(UtilsMage.GetSpellCastExp(_currentSpell.SpellSO.SpellType));

        // uppdate bar ui
        barSpell.SetCurrentValue(_currentSpell.CurrentLearnPoints);
        */
        // save
        SaveNecromancerData();
    }

    public void ExternalAttack()
    {
        // cast spell
        //SpawnSpell();
    }
    /*
    private void SpawnSpell()
    {
        GameObject spawned = Instantiate(_spellPrefab, transform.position, Quaternion.identity);
        if (spawned.TryGetComponent(out SpellMage spellMage))
        {
            if (spellMage.DoesMove)
            {
                spellMage.SetPositions(castPosition.position, new Vector2(_fightPositions.position.x, castPosition.position.y));
            }
            else
            {
                spellMage.SetPositions(new Vector2(_fightPositions.position.x, castPosition.position.y), new Vector2(_fightPositions.position.x, castPosition.position.y));
            }
            spellMage.Perform();
        }
    }*/


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

    public void SaveNecromancerData()
    {
        PlayerManager.Instance.UpdateNecromancerData(PlayerData);
        PlayerManager.Instance.SaveNecromancerData();
    }

    #endregion

    #region HANDLE EVENTS FROM NECROMANCER DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveNecromancerData();
    }

    private void OnStatChangeNecromancer(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
