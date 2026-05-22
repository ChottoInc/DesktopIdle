using TMPro;
using UnityEngine;

public class UIBountyRequestPrefab : MonoBehaviour
{
    [SerializeField] int slot;
    [SerializeField] UITabQuestsBounties tabBounties;

    [Header("Texts")]
    [SerializeField] TMP_Text textSelectBounty;

    private void Awake()
    {
        RefreshTexts();
    }

    private void RefreshTexts()
    {
        textSelectBounty.text = UtilsText.AllTextDictionary[UtilsText.text_button_selectbounty];
    }

    public void OnButtonChoose()
    {
        AudioManager.Instance.PlayClickUI();

        tabBounties.OpenBountiesList(slot);
    }
}
