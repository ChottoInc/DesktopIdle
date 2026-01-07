using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quest/Story Data", fileName = "QuestStoryData_")]
public class QuestStorySO : ScriptableObject
{
    [SerializeField] string uniqueId;

    [SerializeField] UtilsQuest.QuestData questData;

    [SerializeField] QuestStorySO[] nexts;


    public string UniqueId => uniqueId;
    public UtilsQuest.QuestData QuestData => questData;
    public QuestStorySO[] Nexts => nexts;

}
