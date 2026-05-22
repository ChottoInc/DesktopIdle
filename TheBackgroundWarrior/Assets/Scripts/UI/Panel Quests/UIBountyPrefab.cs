using TMPro;
using UnityEngine;

public class UIBountyPrefab : MonoBehaviour
{
    [SerializeField] TMP_Text textBounty;
    [SerializeField] TMP_Text textAccept;

    private UIPanelBountiesList panelBounties;

    private string id;

    public void Setup(UIPanelBountiesList panelBounties, string id, UtilsQuest.QuestData data)
    {
        this.panelBounties = panelBounties;
        this.id = id;

        // set reward
        int rewardAmount = data.rewardAmount;
        string rewardString = string.Format(UtilsText.AllTextDictionary[UtilsText.text_quest_reward_bounty], rewardAmount);

        // set description
        string questDesc = string.Format("{0}\n{1}", UtilsQuest.GetQuestDescription(data), rewardString);

        

        textBounty.text = questDesc;

        textAccept.text = UtilsText.AllTextDictionary[UtilsText.text_button_accept];
    }

    public void OnButtonAccept()
    {
        panelBounties.OnBountyAccepted(id);
    }
}
