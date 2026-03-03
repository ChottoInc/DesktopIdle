using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabWarriorMap : MonoBehaviour
{
    [SerializeField] TMP_Text textMapName;
    [SerializeField] TMP_Text textMapStage;

    [Space(10)]
    [SerializeField] Button buttonMap;
    [SerializeField] GameObject availableBarrier;

    [Space(10)]
    [SerializeField] Image imageToHighlight;
    [SerializeField] Color colorSelected;

    private UITabJobWarrior uiTabWarrior;
    private CombatMapSO mapSO;

    public void Setup(UITabJobWarrior uiTabWarrior, CombatMapSO mapSO)
    {
        this.uiTabWarrior = uiTabWarrior;
        this.mapSO = mapSO;

        textMapName.text = mapSO.MapName;

        CombatMapSaveData mapData = SettingsManager.Instance.GetCombatMapSaveData(mapSO);
        textMapStage.text = string.Format("Stage: {0}/{1}", mapData.currentStage, mapSO.Stages);

        if (PlayerManager.Instance.PlayerFightData.AvailableMaps.Contains(mapSO.IdMap))
        {
            availableBarrier.SetActive(false);
        }
        else
        {
            availableBarrier.SetActive(true);
            buttonMap.interactable = false;
            textMapStage.gameObject.SetActive(false);
        }
    }

    public void OnButtonClick()
    {
        uiTabWarrior.OnMapSelected(mapSO.MapSceneName, mapSO.IdMap);

        imageToHighlight.color = colorSelected;
    }
}
