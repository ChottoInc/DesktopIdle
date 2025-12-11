using UnityEngine;
using UnityEngine.UI;

public class UIPanelAutoBattle : MonoBehaviour
{
    [SerializeField] Toggle toggleAutoBattle;

    public void Start()
    {
        toggleAutoBattle.SetIsOnWithoutNotify(SettingsManager.Instance.IsAutoBattleOn);
    }


    public void OnToggleAutoBattle(bool isOn)
    {
        SettingsManager.Instance.SetIsAutoBattle(isOn);
    }
}
