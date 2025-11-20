using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Combat/Combat Map Data", fileName = "CombatMapData_")]
public class CombatMapSO : ScriptableObject
{
    [SerializeField] UtilsCombatMap.MapDifficulty mapDifficulty;
    [SerializeField] int baseEnemyLevel;
    [SerializeField] int stages;

    public UtilsCombatMap.MapDifficulty MapDifficuty => mapDifficulty;
    public int BaseEnemyLevel => baseEnemyLevel;
    public int Stages => stages;
}
