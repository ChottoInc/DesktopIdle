using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Mage/Spell Data", fileName = "SpellData_")]
public class SpellSO : ListableGameDataSO
{
    [Space(10)]
    [SerializeField] UtilsMage.MageSpellType spellType;
    [SerializeField] UtilsMage.SpellTargetType _targetType;
    [SerializeField] UtilsMage.SpellCombatData _combatData;

    [Space(10)]
    [SerializeField] Sprite sprite;
    [SerializeField] string spellNameTextId;
    [SerializeField] string spellName;

    [Space(10)]
    [SerializeField] string spellDescTextId;

    [TextArea]
    [SerializeField] string spellDesc;

    [Header("Prefab")]
    [SerializeField] GameObject prefab;

    [Header("Settings")]
    [SerializeField] float baseLearningPoints;
    [SerializeField] float baseCooldownCastWarrior;
    [SerializeField] int maxRank;


    public UtilsMage.MageSpellType SpellType => spellType;
    public UtilsMage.SpellTargetType TargetType => _targetType;
    public UtilsMage.SpellCombatData CombatData => _combatData;

    public Sprite Sprite => sprite;

    public string SpellName
    {
        get
        {
            string res = UtilsText.AllText[spellNameTextId];
            if (res != null) return res; else return spellName;
        }
    }

    public string SpellDesc
    {
        get
        {
            string res = UtilsText.AllText[spellDescTextId];
            if (res != null) return res; else return spellDesc;
        }
    }

    public GameObject Prefab => prefab;

    public float BaseLearningPoints => baseLearningPoints;
    public float BaseCooldownCastWarrior => baseCooldownCastWarrior;
    public int MaxRank => maxRank;
}
