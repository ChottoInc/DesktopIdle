using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestPrefab : MonoBehaviour
{
    [SerializeField] TMP_Text textQuest;

    [Space(10)]
    [SerializeField] GameObject panelButtonClaim;
    [SerializeField] Button buttonClaim;


    private UITabQuestsStory questStoryWindow;
    private UITabQuestsBounties questBountiesWindow;
    private UITabQuestsDaily questDailyWindow;

    private UtilsQuest.QuestType questType;

    private string storyQuestId;

    private string bountyQuestId;

    private string dailyQuestId;

    // Used to check if the daily quest is complete
    public bool IsCleared => buttonClaim.interactable;

    private int rewardAmount;



    public void Setup(UITabQuestsStory questWindow, UtilsQuest.QuestType questType, string storyQuestId, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questDataProgress)
    {
        questStoryWindow = questWindow;

        SetupQuest(questType, storyQuestId, questData, questDataProgress);
    }

    public void Setup(UITabQuestsBounties questWindow, UtilsQuest.QuestType questType, string bountyQuestId, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questDataProgress)
    {
        questBountiesWindow = questWindow;

        SetupQuest(questType, bountyQuestId, questData, questDataProgress);
    }

    public void Setup(UITabQuestsDaily questWindow, UtilsQuest.QuestType questType, string storyQuestId, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questDataProgress)
    {
        questDailyWindow = questWindow;

        SetupQuest(questType, storyQuestId, questData, questDataProgress);
    }

    private void SetupQuest(UtilsQuest.QuestType questType, string questId, UtilsQuest.QuestData questData, UtilsQuest.QuestDataProgress questDataProgress)
    {
        this.questType = questType;

        switch (questType)
        {
            case UtilsQuest.QuestType.Story: storyQuestId = questId; break;
            case UtilsQuest.QuestType.Bounties: bountyQuestId = questId; break;
            case UtilsQuest.QuestType.Daily: 
                dailyQuestId = questId; 
                panelButtonClaim.SetActive(false);
                break;
        }

        // set description
        string questDesc = UtilsQuest.GetQuestProgress(questData, questDataProgress);
        textQuest.text = questDesc;

        // set claimable
        buttonClaim.interactable = CanClaim(questData, questDataProgress);

        // set reward
        rewardAmount = questData.rewardAmount;
    }

    private bool CanClaim(UtilsQuest.QuestData data, UtilsQuest.QuestDataProgress progress)
    {
        switch (data.questObjectiveType)
        {
            default:
            case UtilsQuest.QuestObjectiveType.Kill:
                return HandleCounterQuestCheck(data.amountKill, progress.progressCounter);

            case UtilsQuest.QuestObjectiveType.Obtain:
                return HandleCounterQuestCheck(data.amountObtain, progress.progressCounter);

            case UtilsQuest.QuestObjectiveType.LevelUp:
                return HandleCounterQuestCheck(data.amountStat, progress.progressCounter);
        }
    }

    private bool HandleCounterQuestCheck(int counter, int progress)
    {
        if (progress >= counter) return true;
        return false;
    }


    public void OnButtonClaim()
    {
        // add reward to player
        PlayerManager.Instance.Inventory.AddBits(rewardAmount);

        // switch behaviour for quest type
        switch (questType)
        {
            case UtilsQuest.QuestType.Story:
                // set clear and update new paths
                QuestManager.Instance.SetStoryQuestCleared(storyQuestId);
                QuestManager.Instance.UpdateStoryQuests();

                // update quest window
                questStoryWindow.FillQuests();

                break;

            case UtilsQuest.QuestType.Bounties:
                // set clear bounty
                QuestManager.Instance.SetBountyQuestCleared(bountyQuestId);
                QuestManager.Instance.UpdateBountyQuests();

                // update quest window
                questBountiesWindow.FillQuests();
                break;
        }
        
        // save
        PlayerManager.Instance.SaveInventoryData();
        QuestManager.Instance.SaveQuestsData();
    }
}
