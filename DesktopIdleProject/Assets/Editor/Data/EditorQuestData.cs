using UnityEditor;
using static UtilsQuest;

[CustomEditor(typeof(QuestStorySO))]
public class EditorQuestData : Editor
{
    private QuestStorySO _quest;

    private SerializedProperty s_uniqueId;
    private SerializedProperty s_questData;
    private SerializedProperty s_nexts;



    private bool showQuestData = true;

    private SerializedProperty s_questType;

    // --------- Quest Kill ---------
    private SerializedProperty s_questKillSpecific;

    // --- Specific
    private SerializedProperty s_monsterId;

    private SerializedProperty s_amountKill;

    // --------- Quest Obtain ---------
    private SerializedProperty s_itemType;
    private SerializedProperty s_questObtainSpecific;

    // --- Specific
    private SerializedProperty  s_itemId;

    private SerializedProperty s_amountObtain;

    // --------- Quest Level Up ---------
    private SerializedProperty s_questLevelUpSpecific;

    // --- Specific
    private SerializedProperty s_statId;

    private SerializedProperty s_amountStat;


    // --------- Reward ---------
    private SerializedProperty s_rewardAmount;


    private void OnEnable()
    {
        s_uniqueId = serializedObject.FindProperty("uniqueId");
        s_questData = serializedObject.FindProperty("questData");
        s_nexts = serializedObject.FindProperty("nexts");


        s_questType = s_questData.FindPropertyRelative("questType");

        s_questKillSpecific = s_questData.FindPropertyRelative("questKillSpecific");
        s_monsterId = s_questData.FindPropertyRelative("monsterId");
        s_amountKill = s_questData.FindPropertyRelative("amountKill");

        s_itemType = s_questData.FindPropertyRelative("itemType");
        s_questObtainSpecific = s_questData.FindPropertyRelative("questObtainSpecific");
        s_itemId = s_questData.FindPropertyRelative("itemId");
        s_amountObtain = s_questData.FindPropertyRelative("amountObtain");

        s_questLevelUpSpecific = s_questData.FindPropertyRelative("questLevelUpSpecific");
        s_statId = s_questData.FindPropertyRelative("statId");
        s_amountStat = s_questData.FindPropertyRelative("amountStat");

        s_rewardAmount = s_questData.FindPropertyRelative("rewardAmount");
    }

    public override void OnInspectorGUI()
    {
        _quest = (QuestStorySO)target;

        serializedObject.Update();


        EditorGUILayout.PropertyField(s_uniqueId);

        showQuestData = EditorGUILayout.Foldout(
            showQuestData,
            "Quest Data",
            true
        );

        if (showQuestData)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(s_questType);

            switch (_quest.QuestData.questType)
            {
                case QuestType.Kill:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_questKillSpecific);

                    if (_quest.QuestData.questKillSpecific)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(s_monsterId);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_amountKill);

                    break;

                case QuestType.Obtain:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_itemType);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_questObtainSpecific);

                    if (_quest.QuestData.questObtainSpecific)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(s_itemId);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_amountObtain);

                    break;

                case QuestType.LevelUp:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_questLevelUpSpecific);

                    if (_quest.QuestData.questLevelUpSpecific)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(s_statId);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(s_amountStat);

                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(s_rewardAmount);
        }

        


        EditorGUILayout.PropertyField(s_nexts);


        serializedObject.ApplyModifiedProperties();
    }
}
