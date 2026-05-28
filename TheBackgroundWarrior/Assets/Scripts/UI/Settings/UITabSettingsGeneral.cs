using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsGeneral : UITabWindow
{
    [SerializeField] Slider sliderMaster;
    [SerializeField] TMP_Dropdown dropdownLanguage;

    [Header("Texts")]
    [SerializeField] TMP_Text textVolume;
    [SerializeField] TMP_Text textLanguage;
    [SerializeField] TMP_Text textExit;
    [SerializeField] TMP_Text textButtonTitleScreen;
    [SerializeField] TMP_Text textButtonQuit;

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        sliderMaster.SetValueWithoutNotify(SettingsManager.Instance.MasterVolume);

        dropdownLanguage.ClearOptions();
        List<TMP_Dropdown.OptionData> listLanguage = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData(UtilsText.AllTextDictionary[UtilsText.text_settings_general_lang_english]),
            new TMP_Dropdown.OptionData(UtilsText.AllTextDictionary[UtilsText.text_settings_general_lang_italian])
        };

        dropdownLanguage.AddOptions(listLanguage);

        dropdownLanguage.SetValueWithoutNotify((int)SettingsManager.Instance.CurrentLanguage);

        RefreshTexts();
    }


    private void RefreshTexts()
    {
        textVolume.text = UtilsText.AllTextDictionary[UtilsText.text_settings_general_titlevolume];
        textLanguage.text = UtilsText.AllTextDictionary[UtilsText.text_settings_general_titlelanguage];
        textExit.text = UtilsText.AllTextDictionary[UtilsText.text_settings_general_titleexit];
        textButtonTitleScreen.text = UtilsText.AllTextDictionary[UtilsText.text_settings_general_button_titlescreen];
        textButtonQuit.text = UtilsText.AllTextDictionary[UtilsText.text_button_quit];
    }



    public void OnMasterChange(float value)
    {
        SettingsManager.Instance.SetMasterVolume(value);
    }

    public void OnLanguageChange(int index)
    {
        SettingsManager.Instance.SetLanguage((UtilsGeneral.Language)index);
        RefreshTexts();
    }




    public async void OnButtonTitleScreen()
    {
        string question = UtilsText.AllTextDictionary[UtilsText.text_yesno_question_titlescreen];

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_YESNO;
        tooltipData.text = question;

        bool confirm = await UITooltipManager.Instance.ShowPanelYesNoCallback(tooltipData, UITooltipManager.Instance.CenterPoint.position, true);

        if(confirm)
        {

            SceneLoaderManager.Instance.LoadHome();
        }
    }

    public async void OnButtonQuit()
    {
        string question = UtilsText.AllTextDictionary[UtilsText.text_yesno_question_quitgame];

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_YESNO;
        tooltipData.text = question;

        bool confirm = await UITooltipManager.Instance.ShowPanelYesNoCallback(tooltipData, UITooltipManager.Instance.CenterPoint.position, true);

        if (confirm)
        {
            Application.Quit();
        }
    }
}
