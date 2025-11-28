using TMPro;
using UnityEngine;

public class UITabWarriorMap : MonoBehaviour
{
    [SerializeField] TMP_Text textMapName;

    private UITabJobWarrior uiTabWarrior;
    private CombatMapSO mapSO;

    public void Setup(UITabJobWarrior uiTabWarrior, CombatMapSO mapSO)
    {
        this.uiTabWarrior = uiTabWarrior;
        this.mapSO = mapSO;

        textMapName.text = mapSO.MapName;
    }

    public void OnButtonClick()
    {
        uiTabWarrior.OnMapSelected(mapSO.MapName, mapSO.IdMap);
    }
}
