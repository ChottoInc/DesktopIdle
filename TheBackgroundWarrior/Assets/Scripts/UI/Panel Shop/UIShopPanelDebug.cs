using TMPro;
using UnityEngine;

public class UIShopPanelDebug : MonoBehaviour
{
    [SerializeField] TMP_InputField inputCode;

    [Header("Texts")]
    [SerializeField] TMP_Text textDebug;
    [SerializeField] TMP_Text textButtonDebug;

    private void Awake()
    {
        RefreshTexts();
    }

    private void RefreshTexts()
    {
        textDebug.text = UtilsText.AllTextDictionary[UtilsText.text_shop_insertdebug];
        textButtonDebug.text = UtilsText.AllTextDictionary[UtilsText.text_button_debug];
    }

    public void OnButtonRedeem()
    {
        AudioManager.Instance.PlayClickUI();

        bool redeemSuccess = false;

        switch (inputCode.text)
        {
            default: Debug.Log("Redeem denied"); break;

            case UtilsShop.REDEEM_ERIS_CODE:

                if (!ShopManager.Instance.HasRedeemedErisCode)
                {
                    redeemSuccess = true;
                    ShopManager.Instance.SetRedeemCode(UtilsShop.ID_REDEEM_ERIS_CODE);
                    ShopManager.Instance.SaveShopData();
                    Debug.Log("Redeem success: " + UtilsShop.REDEEM_ERIS_CODE);
                }
                break;
        }

        if(!redeemSuccess)
        {
            Debug.Log("Redeem denied");
            // little ui animation of button shaking if not success?
        }
    }
}
