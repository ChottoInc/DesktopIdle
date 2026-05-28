using TMPro;
using UnityEngine;

public class UITabSettings : UITabWindow
{
    [SerializeField] TabManager tabManager;

    [Header("Texts")]
    [SerializeField] TMP_Text textTitle;
    [SerializeField] TMP_Text textFilterGeneral;
    [SerializeField] TMP_Text textFilterGameplay;
    [SerializeField] TMP_Text textFilterVideo;
    [SerializeField] TMP_Text textFilterCredits;
    [SerializeField] TMP_Text textFilterHelp;

    private void Awake()
    {
        SettingsManager.Instance.OnLanguageChange += RefreshTexts;
    }

    private void OnDestroy()
    {
        SettingsManager.Instance.OnLanguageChange -= RefreshTexts;
    }

    public override void Open()
    {
        base.Open();

        tabManager.SelectFirstTab();

        RefreshTexts();
    }

    private void RefreshTexts()
    {
        textTitle.text = UtilsText.AllTextDictionary[UtilsText.text_title_settings];
        textFilterGeneral.text = UtilsText.AllTextDictionary[UtilsText.text_button_settings_filter_general];
        textFilterGameplay.text = UtilsText.AllTextDictionary[UtilsText.text_button_settings_filter_gameplay];
        textFilterVideo.text = UtilsText.AllTextDictionary[UtilsText.text_button_settings_filter_video];
        textFilterCredits.text = UtilsText.AllTextDictionary[UtilsText.text_button_settings_filter_credits];
        textFilterHelp.text = UtilsText.AllTextDictionary[UtilsText.text_button_settings_filter_help];
    }

    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
    }
}
