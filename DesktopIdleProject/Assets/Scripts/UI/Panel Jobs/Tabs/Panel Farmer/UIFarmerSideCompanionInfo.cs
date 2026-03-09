using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFarmerSideCompanionInfo : MonoBehaviour
{
    [SerializeField] Sprite spriteEmpty;
    [SerializeField] Image imageIconCompanion;
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textLevel;

    private CompanionData companionData;

    public void Setup(CompanionData companionData)
    {
        if(companionData == null)
        {
            imageIconCompanion.sprite = spriteEmpty;

            textName.text = "Empty";
            textLevel.gameObject.SetActive(false);
        }
        else
        {
            this.companionData = companionData;

            imageIconCompanion.sprite = companionData.CompanionSO.IconCompanion;

            textName.text = companionData.CompanionSO.CompanionName;
            textLevel.text = companionData.CurrentLevel.ToString();
            textLevel.gameObject.SetActive(true);
        }
            
    }
}
