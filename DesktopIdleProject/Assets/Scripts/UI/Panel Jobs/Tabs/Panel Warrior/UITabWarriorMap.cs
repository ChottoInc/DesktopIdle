using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabWarriorMap : MonoBehaviour
{
    [SerializeField] TMP_Text textMapName;

    [Space(10)]
    [SerializeField] Button buttonMap;
    [SerializeField] GameObject availableBarrier;

    private UITabJobWarrior uiTabWarrior;
    private CombatMapSO mapSO;

    public void Setup(UITabJobWarrior uiTabWarrior, CombatMapSO mapSO)
    {
        this.uiTabWarrior = uiTabWarrior;
        this.mapSO = mapSO;

        textMapName.text = mapSO.MapName;

        if (PlayerManager.Instance.PlayerFightData.AvailableMaps.Contains(mapSO.IdMap))
        {
            availableBarrier.SetActive(false);
        }
        else
        {
            availableBarrier.SetActive(true);
            buttonMap.interactable = false;
        }
    }

    public void OnButtonClick()
    {
        uiTabWarrior.OnMapSelected(mapSO.MapSceneName, mapSO.IdMap);
    }
}
