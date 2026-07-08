using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Concocotion Data", fileName = "ConcoctionData_")]
public class ConcoctionSO : ItemSO
{
    [SerializeField] bool _permanent;
    [SerializeField] UtilsAlchemist.PermaStat _permaStat;

    [Space(10)]
    [SerializeField] UtilsBuffs.BuffType _buff;
    [SerializeField] float _buffDuration;

    public bool Permanent => _permanent;
    public UtilsAlchemist.PermaStat PermaStat => _permaStat;

    public UtilsBuffs.BuffType Buff => _buff;
    public float Duration => _buffDuration;
}
