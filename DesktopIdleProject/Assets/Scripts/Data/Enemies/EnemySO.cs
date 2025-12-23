using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy/Enemy Data", fileName = "EnemyData_")]
public class EnemySO : ScriptableObject
{
    [SerializeField] string enemyPoolName;
    [SerializeField] int id;

    [Space(10)]
    [SerializeField] float baseMaxHp = 30f;
    [SerializeField] float baseAtk = 3f;
    [SerializeField] float baseDef = 1f;
    [SerializeField] float baseCritDmg = 1.5f;


    public string EnemyPoolName => enemyPoolName;
    public int Id => id;

    public float BaseMaxHp => baseMaxHp;
    public float BaseAtk => baseAtk;
    public float BaseDef => baseDef;
    public float BaseCritDmg => baseCritDmg;



    public override bool Equals(object other)
    {
        EnemySO otherEnemy = other as EnemySO;
        return id == otherEnemy.id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
