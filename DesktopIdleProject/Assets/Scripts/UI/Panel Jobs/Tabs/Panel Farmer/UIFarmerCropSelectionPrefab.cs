using UnityEngine;
using UnityEngine.UI;

public class UIFarmerCropSelectionPrefab : MonoBehaviour
{
    [SerializeField] Image imageCrop;
    [SerializeField] Transform tooltipPosition;

    private bool isShowingTooltip;

    private UIFarmerPanelSelectionCrop panelSelection;
    private CropSO cropSO;

    public void Setup(UIFarmerPanelSelectionCrop panelSelection, CropSO cropSO)
    {
        this.panelSelection = panelSelection;
        this.cropSO = cropSO;

        imageCrop.sprite = cropSO.SpriteSeed;
    }

    public void OnPointerEnter()
    {
        if (isShowingTooltip) return;

        isShowingTooltip = true;

        string possibleCompanions = string.Empty;

        for (int i = 0; i < cropSO.AttractedCompanions.Length; i++)
        {
            possibleCompanions += cropSO.AttractedCompanions[i].CompanionName;

            // add new line only when not last possible
            if(i < cropSO.AttractedCompanions.Length - 1)
            {
                possibleCompanions += "\n";
            }
        }

        string text = string.Format(
            "{0}\n" +
            "Base growth time: {1}\n" +
            "Attracts:\n" +
            "{2}",
            cropSO.CropName,
            cropSO.BaseGrowthTime,
            possibleCompanions);


        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = text;
        UITooltipManager.Instance.Show(tooltipData, tooltipPosition.position, true);
    }

    public void OnPointerExit()
    {
        if (!isShowingTooltip) return;

        isShowingTooltip = false;

        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }

    public void OnCropSelected()
    {
        panelSelection.OnCropSelected(cropSO);
    }
}
