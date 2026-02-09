using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Metal Data", fileName = "MetalData_")]
public class MetalSO : ItemSO
{
    [Space(10)]
    [SerializeField] int requiredOres;

    public int RequiredOres => requiredOres;
}
