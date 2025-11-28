using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    private const float PER_STAGE_GAIN_LEVEL = 1f;
    private const float PER_ENEMY_GAIN_LEVEL = 0.0202f;

    /*
    private const float PER_STAGE_GAIN_MAXHP = 1.12f;
    private const float PER_STAGE_GAIN_ATK = 1.1f;
    private const float PER_STAGE_GAIN_DEF = 1.08f;

    private const float PER_SUBSTAGE_MULTIPLIER_MAXHP = 1.05f;
    private const float PER_SUBSTAGE_MULTIPLIER_ATK = 1.03f;
    private const float PER_SUBSTAGE_MULTIPLIER_DEF = 1.02f;


    private float baseMaxHp = 40;
    private float baseAtk = 6;
    private float baseDef = 3;
    */


    private int currentLevel;

    private float maxHp;
    private float currentHp;

    private float currentAtk;
    private float currentDef;

    private float currentAtkSpd;

    private float currentCritRate;
    private float currentCritDmg;


    private CombatMapSO mapSO;


    public int CurrentLevel => currentLevel;

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;

    public float CurrentAtk => currentAtk;
    public float CurrentDef => currentDef;

    public float CurrentAtkSpd => currentAtkSpd;
    public float CurrentCritRate => currentCritRate;
    public float CurrentCritDmg => currentCritDmg;






    public EnemyData(CombatMapSO mapSO)
    {
        this.mapSO = mapSO;

        currentLevel = Mathf.FloorToInt(CalculateLevel());

        maxHp = CalculateMaxHp();
        currentHp = maxHp;

        currentAtk = CalculateAtk();
        currentDef = CalculateDef();

        currentAtkSpd = 0.8f;

        currentCritRate = 0.02f;
        currentCritDmg = 1.3f;
    }

    private float CalculateLevel()
    {
        float result;

        result = mapSO.BaseEnemyLevel +
            (StageManager.Instance.CurrentStage - 1) * PER_STAGE_GAIN_LEVEL +
            (StageManager.Instance.CurrentEnemyIndex - 1) * PER_ENEMY_GAIN_LEVEL;

        return result;
    }

    private float CalculateMaxHp()
    {
        return 20f * Mathf.Pow(currentLevel, 1.1f);
    }

    private float CalculateAtk()
    {
        return 3.5f * Mathf.Pow(currentLevel, 1.05f);
        //return 20f * Mathf.Pow(currentLevel, 1.05f);
    }

    private float CalculateDef()
    {
        return 1.2f * Mathf.Pow(currentLevel, 1.05f);
    }

    /*
    private float CalculateMaxHp()
    {
        float result;

        result =
            baseMaxHp *
            Mathf.Pow(PER_STAGE_GAIN_MAXHP, StageManager.Instance.CurrentStage - 1) *
            Mathf.Pow(PER_SUBSTAGE_MULTIPLIER_MAXHP, StageManager.Instance.CurrentEnemyIndex - 1) *
            StageManager.Instance.CurrentPrestige;

        return result;
    }

    private float CalculateAtk()
    {
        float result;

        result =
            baseAtk *
            Mathf.Pow(PER_STAGE_GAIN_ATK, StageManager.Instance.CurrentStage - 1) *
            Mathf.Pow(PER_SUBSTAGE_MULTIPLIER_ATK, StageManager.Instance.CurrentEnemyIndex - 1) *
            StageManager.Instance.CurrentPrestige;

        return result;
    }

    private float CalculateDef()
    {
        float result;

        result =
            baseDef *
            Mathf.Pow(PER_STAGE_GAIN_DEF, StageManager.Instance.CurrentStage - 1) *
            Mathf.Pow(PER_SUBSTAGE_MULTIPLIER_DEF, StageManager.Instance.CurrentEnemyIndex - 1) *
            StageManager.Instance.CurrentPrestige;

        return result;
    }

    */

    #region COMABT SYSTEM

    public void TakeDamage(PlayerFightData data)
    {
        // can't take less than 0 or it will cure

        float baseDamage = data.CurrentAtk;
        float total;

        if(Random.value <= data.CurrentCritRate)
        {
            baseDamage *= data.CurrentCritDmg;
        }

        total = Mathf.Max(0f, baseDamage - currentDef);

        // subtract total to hp
        currentHp -= total;

        if(currentHp <= 0f)
        {
            currentHp = 0;
        }
    }

    public void SetDead()
    {
        currentHp = 0;
    }

    #endregion
}
