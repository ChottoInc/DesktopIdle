using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Map To Cards Data", fileName = "MapToCardsData_")]
public class MapToCardsSO : ScriptableObject
{
    [SerializeField] CombatMapSO mapSO;
    [SerializeField] UtilsGeneral.GeneralChances<CardSO>[] _possibleCards;

    public CombatMapSO MapSO => mapSO;
    public UtilsGeneral.GeneralChances<CardSO>[] PossibleCards => _possibleCards;
}
