using UnityEditor;

[CustomEditor(typeof(SpellSO))]
public class EditorSpellSO : Editor
{
    private SpellSO _spellSO;


    private SerializedProperty _id;

    private SerializedProperty _spellType;
    private SerializedProperty _targetType;
    private SerializedProperty _spellCombatData;

    private bool showSpellCombatData = true;

    // damage based on max hp enemy
    private SerializedProperty _percDamage;
    private SerializedProperty _percAddDamagePerLevel;
                               
    // radius                  
    private SerializedProperty _radius;
    private SerializedProperty _percAddRadiusPerLevel;
                               
    // chill wind              
    private SerializedProperty _percMoreDamageFromSpells;
    private SerializedProperty _percAddMoreDamageFromSpellsPerLevel;
                               
    // poison gas              
    private SerializedProperty _percLifesteal;
    private SerializedProperty _percAddLifestealPerLevel;
                               
    // zap                     
    private SerializedProperty _maxBounces;
    private SerializedProperty _addBouncesPerLevel;


    private SerializedProperty _sprite;
    private SerializedProperty _spellNameTextId;
    private SerializedProperty _spellName;
                               
    private SerializedProperty _spellDescTextId;
                               
    private SerializedProperty _spellDesc;
                               
    private SerializedProperty _prefab;
                               
    private SerializedProperty _baseLearningPoints;
    private SerializedProperty _baseCooldownCastWarrior;
    private SerializedProperty _maxRank;

    private void OnEnable()
    {
        _id = serializedObject.FindProperty("id");
        _spellType = serializedObject.FindProperty("spellType");
        _targetType = serializedObject.FindProperty("_targetType");
        _spellCombatData = serializedObject.FindProperty("_combatData");


        _percDamage = _spellCombatData.FindPropertyRelative("percDamage");
        _percAddDamagePerLevel = _spellCombatData.FindPropertyRelative("percAddDamagePerLevel");

        _radius = _spellCombatData.FindPropertyRelative("radius");
        _percAddRadiusPerLevel = _spellCombatData.FindPropertyRelative("percAddRadiusPerLevel");

        _percMoreDamageFromSpells = _spellCombatData.FindPropertyRelative("percMoreDamageFromSpells");
        _percAddMoreDamageFromSpellsPerLevel = _spellCombatData.FindPropertyRelative("percAddMoreDamageFromSpellsPerLevel");

        _percLifesteal = _spellCombatData.FindPropertyRelative("percLifesteal");
        _percAddLifestealPerLevel = _spellCombatData.FindPropertyRelative("percAddLifestealPerLevel");

        _maxBounces = _spellCombatData.FindPropertyRelative("maxBounces");
        _addBouncesPerLevel = _spellCombatData.FindPropertyRelative("addBouncesPerLevel");


        _sprite = serializedObject.FindProperty("sprite");
        _spellNameTextId = serializedObject.FindProperty("spellNameTextId");
        _spellName = serializedObject.FindProperty("spellName");

        _spellDescTextId = serializedObject.FindProperty("spellDescTextId");

        _spellDesc = serializedObject.FindProperty("spellDesc");

        _prefab = serializedObject.FindProperty("prefab");

        _baseLearningPoints = serializedObject.FindProperty("baseLearningPoints");
        _baseCooldownCastWarrior = serializedObject.FindProperty("baseCooldownCastWarrior");
        _maxRank = serializedObject.FindProperty("maxRank");
    }

    public override void OnInspectorGUI()
    {
        _spellSO = (SpellSO)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(_id);
        EditorGUILayout.PropertyField(_spellType);
        EditorGUILayout.PropertyField(_targetType);

        showSpellCombatData = EditorGUILayout.Foldout(
            showSpellCombatData,
            "Combat Data",
            true
        );

        if (showSpellCombatData)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_percDamage);
            EditorGUILayout.PropertyField(_percAddDamagePerLevel);

            EditorGUILayout.Space();

            switch (_spellSO.SpellType)
            {
                case UtilsMage.MageSpellType.Explosion:
                    EditorGUILayout.PropertyField(_radius);
                    EditorGUILayout.PropertyField(_percAddRadiusPerLevel);
                    break;

                case UtilsMage.MageSpellType.ChillWind:
                    EditorGUILayout.PropertyField(_radius);
                    EditorGUILayout.PropertyField(_percAddRadiusPerLevel);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_percMoreDamageFromSpells);
                    EditorGUILayout.PropertyField(_percAddMoreDamageFromSpellsPerLevel);
                    break;

                case UtilsMage.MageSpellType.PoisonGas:
                    EditorGUILayout.PropertyField(_radius);
                    EditorGUILayout.PropertyField(_percAddRadiusPerLevel);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_percLifesteal);
                    EditorGUILayout.PropertyField(_percAddLifestealPerLevel);
                    break;

                case UtilsMage.MageSpellType.Zap:
                    EditorGUILayout.PropertyField(_radius);
                    EditorGUILayout.PropertyField(_percAddRadiusPerLevel);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_maxBounces);
                    EditorGUILayout.PropertyField(_addBouncesPerLevel);
                    break;

            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_sprite);
        EditorGUILayout.PropertyField(_spellNameTextId);
        EditorGUILayout.PropertyField(_spellName);

        EditorGUILayout.PropertyField(_spellDescTextId);

        EditorGUILayout.PropertyField(_spellDesc);

        EditorGUILayout.PropertyField(_prefab);

        EditorGUILayout.PropertyField(_baseLearningPoints);
        EditorGUILayout.PropertyField(_baseCooldownCastWarrior);
        EditorGUILayout.PropertyField(_maxRank);


        serializedObject.ApplyModifiedProperties();
    }
}
