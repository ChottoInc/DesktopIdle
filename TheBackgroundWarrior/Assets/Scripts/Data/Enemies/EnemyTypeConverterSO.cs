using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy/Type Converter", fileName = "EnemyTypeConverter_")]
public class EnemyTypeConverterSO : ListableGameDataSO
{
    [SerializeField] UtilsEnemy.EnemyType enemyType;
    [SerializeField] string enemyPoolName;
    [SerializeField] string enemyNameTextId;
    [SerializeField] string enemyNamePluralTextId;
    [SerializeField] string enemyName;

    public override int Id => (int)enemyType;

    public UtilsEnemy.EnemyType EnemyType => enemyType;

    public string EnemyPoolName => enemyPoolName.ToLower();

    public string EnemyName
    {
        get
        {
            string res = UtilsText.AllText[enemyNameTextId];
            if (res != null) return res; else return enemyName;
        }
    }

    public string EnemyNamePlural
    {
        get
        {
            string res = UtilsText.AllText[enemyNamePluralTextId];
            if (res != null) return res; else return enemyName;
        }
    }
}
