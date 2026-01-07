using Kirurobo;
using UnityEngine;

[RequireComponent(typeof(UniWindowController))]
public class WindowSettingsController : MonoBehaviour
{
    private UniWindowController windowController;

    private void Awake()
    {
        windowController = GetComponent<UniWindowController>();


        SettingsManager.Instance.OnAlwaysOnTopChange += SetAlwaysOnTop;
        SettingsManager.Instance.OnClickThroughChange += SetClickThrough;


        windowController.isTopmost = SettingsManager.Instance.IsAlwaysOnTop;
        windowController.isHitTestEnabled = SettingsManager.Instance.IsClickThrough;
    }

    private void OnDestroy()
    {
        SettingsManager.Instance.OnAlwaysOnTopChange -= SetAlwaysOnTop;
        SettingsManager.Instance.OnClickThroughChange -= SetClickThrough;
    }



    private void SetAlwaysOnTop(bool isOn)
    {
        windowController.isTopmost = isOn;
    }

    private void SetClickThrough(bool isOn)
    {
        windowController.isHitTestEnabled = isOn;
    }
}
