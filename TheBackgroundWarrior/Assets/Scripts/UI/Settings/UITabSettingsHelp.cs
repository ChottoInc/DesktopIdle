using TMPro;
using UnityEngine;

public class UITabSettingsHelp : UITabWindow
{
    [SerializeField] UIHelpJobFilter[] filters;
    [SerializeField] TabManager tabManager;

    [Header("Filters")]
    [SerializeField] TMP_Text textFilterWarrior;
    [SerializeField] TMP_Text textFilterMiner;
    [SerializeField] TMP_Text textFilterFisher;
    [SerializeField] TMP_Text textFilterBlacksmith;
    [SerializeField] TMP_Text textFilterFarmer;

    [Header("Jobs")]
    [SerializeField] TMP_Text textHelpWarrior;
    [SerializeField] TMP_Text textHelpMiner;
    [SerializeField] TMP_Text textHelpFisher;
    [SerializeField] TMP_Text textHelpBlacksmith;
    [SerializeField] TMP_Text textHelpFarmer;

    public override void Open()
    {
        base.Open();

        Setup();

        RefreshTexts();
    }

    private void Setup()
    {
        foreach (var filter in filters)
        {
            filter.gameObject.SetActive(PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(filter.Job));
        }

        tabManager.SelectFirstTab();
    }

    private void RefreshTexts()
    {
        textFilterWarrior.text = UtilsText.AllTextDictionary[UtilsText.text_button_help_filter_warrior];
        textFilterMiner.text = UtilsText.AllTextDictionary[UtilsText.text_button_help_filter_miner];
        textFilterFisher.text = UtilsText.AllTextDictionary[UtilsText.text_button_help_filter_fisher];
        textFilterBlacksmith.text = UtilsText.AllTextDictionary[UtilsText.text_button_help_filter_blacksmith];
        textFilterFarmer.text = UtilsText.AllTextDictionary[UtilsText.text_button_help_filter_farmer];

        textHelpWarrior.text = UtilsText.HelpTextDictionary[UtilsText.text_help_warrior];
        textHelpMiner.text = UtilsText.HelpTextDictionary[UtilsText.text_help_miner];
        textHelpFisher.text = UtilsText.HelpTextDictionary[UtilsText.text_help_fisher];
        textHelpBlacksmith.text = UtilsText.HelpTextDictionary[UtilsText.text_help_blacksmith];
        textHelpFarmer.text = UtilsText.HelpTextDictionary[UtilsText.text_help_farmer];
    }
}
