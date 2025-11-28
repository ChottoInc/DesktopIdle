using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Combat/Combat Map Data", fileName = "CombatMapData_")]
public class CombatMapSO : ScriptableObject
{
    [SerializeField] int idMap;
    [SerializeField] string mapName;

    [SerializeField] UtilsCombatMap.MapDifficulty mapDifficulty;
    [SerializeField] int baseEnemyLevel;
    [SerializeField] int stages;

    public int IdMap => idMap;
    public string MapName => mapName;

    public UtilsCombatMap.MapDifficulty MapDifficuty => mapDifficulty;
    public int BaseEnemyLevel => baseEnemyLevel;
    public int Stages => stages;
}
