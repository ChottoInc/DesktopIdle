using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabQuestsDaily : UITabWindow
{
    [SerializeField] GameObject questPrefab;
    [SerializeField] Transform container;

    private List<GameObject> questObjs;

    [Space(10)]
    [SerializeField] Button buttonClaim;

    public override void Open()
    {
        base.Open();

        FillQuests();
    }

    public void FillQuests()
    {
        questObjs = ClearList(questObjs);

        var activeQuests = QuestManager.Instance.ActiveDailyQuests;

        int clearedCounter = 0;

        for (int i = 0; i < activeQuests.Count; i++)
        {
            string currentId = activeQuests[i];

            GameObject prefab = Instantiate(questPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(container);

            prefab.transform.localScale = new Vector3(1, 1, 1);

            if (prefab.TryGetComponent(out UIQuestPrefab obj))
            {
                QuestDailySO so = UtilsQuest.GetDailyQuestById(currentId);
                UtilsQuest.QuestData questData = so.QuestData;

                UtilsQuest.QuestDataProgress questDataProgress = QuestManager.Instance.DictQuestsDailyProgress[currentId];

                obj.Setup(this, UtilsQuest.QuestType.Daily, currentId, questData, questDataProgress);

                if (obj.IsCleared) 
                    clearedCounter++;
            }
            questObjs.Add(prefab);
        }

        // if all daily quests are cleared you can claim the reward
        buttonClaim.interactable = clearedCounter >= activeQuests.Count;
    }

    private List<GameObject> ClearList(List<GameObject> list)
    {
        if (list == null)
        {
            list = new List<GameObject>();
            return list;
        }

        foreach (var item in list)
        {
            Destroy(item);
        }
        list.Clear();
        return list;
    }


    public void OnButtonClaim()
    {
        PlayerManager.Instance.Inventory.AddBits(UtilsQuest.DAILY_BITS_REWARD);
        PlayerManager.Instance.SaveInventoryData();


        QuestManager.Instance.ClearDailyQuests();
        QuestManager.Instance.SaveQuestsData();
    }
}
