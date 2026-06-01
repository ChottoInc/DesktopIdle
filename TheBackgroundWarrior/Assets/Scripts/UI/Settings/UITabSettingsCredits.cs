using TMPro;
using UnityEngine;

public class UITabSettingsCredits : UITabWindow
{
    [Header("Texts")]
    [SerializeField] TMP_Text textPanelMe;
    [SerializeField] TMP_Text textPanelLocalization;
    [SerializeField] TMP_Text textPanelArt;
    [SerializeField] TMP_Text textPanelSound;
    [SerializeField] TMP_Text textPanelFont;

    public override void Open()
    {
        base.Open();

        Setup();

        RefreshTexts();
    }

    private void RefreshTexts()
    {
        textPanelMe.text = UtilsText.AllDictionaries[UtilsText.text_credits_me];
        textPanelLocalization.text = UtilsText.AllDictionaries[UtilsText.text_credits_localization];
        textPanelArt.text = UtilsText.AllDictionaries[UtilsText.text_credits_art];
        textPanelSound.text = UtilsText.AllDictionaries[UtilsText.text_credits_sound];
        textPanelFont.text = UtilsText.AllDictionaries[UtilsText.text_credits_font];
    }

    private void Setup()
    {

    }
}
