using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Farmer/Companion Data", fileName = "CompanionData_")]
public class CompanionSO : ScriptableObject
{
    [SerializeField] int id;

    public int Id => id;
}
