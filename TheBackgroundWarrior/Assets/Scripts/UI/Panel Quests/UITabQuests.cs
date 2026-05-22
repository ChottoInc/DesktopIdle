using TMPro;
using UnityEngine;

public class UITabQuests : UITabWindow
{
    [SerializeField] TabManager tabManager;

    [Header("Texts")]
    [SerializeField] TMP_Text textTitle;
    [SerializeField] TMP_Text textFilterStory;
    [SerializeField] TMP_Text textFilterDaily;
    [SerializeField] TMP_Text textFilterBounty;

    public override void Open()
    {
        base.Open();

        tabManager.SelectFirstTab();

        RefreshTexts();
    }

    private void RefreshTexts()
    {
        textTitle.text = UtilsText.AllTextDictionary[UtilsText.text_title_shop];
        textFilterStory.text = UtilsText.AllTextDictionary[UtilsText.text_button_quests_filter_story];
        textFilterDaily.text = UtilsText.AllTextDictionary[UtilsText.text_button_quests_filter_daily];
        textFilterBounty.text = UtilsText.AllTextDictionary[UtilsText.text_button_quests_filter_bounty];
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();

        base.Close();
    }
}
