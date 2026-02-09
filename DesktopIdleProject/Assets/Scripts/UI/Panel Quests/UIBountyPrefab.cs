using TMPro;
using UnityEngine;

public class UIBountyPrefab : MonoBehaviour
{
    [SerializeField] TMP_Text textBounty;

    private UIPanelBountiesList panelBounties;

    private string id;

    public void Setup(UIPanelBountiesList panelBounties, string id, UtilsQuest.QuestData data)
    {
        this.panelBounties = panelBounties;
        this.id = id;

        // set description
        string questDesc = UtilsQuest.GetQuestDescription(data);
        textBounty.text = questDesc;

        // set reward
        //rewardAmount = questData.rewardAmount;
    }

    public void OnButtonAccept()
    {
        panelBounties.OnBountyAccepted(id);
    }
}
