using Kirurobo;
using System.Collections;
using UnityEngine;
using static Kirurobo.UniWindowController;

public class InitializerManager : MonoBehaviour
{
    [SerializeField] float timerSetup = 1f;
    [SerializeField] UniWindowController windowController;

    private bool isInit;

    private void Awake()
    {
        windowController.OnStateChanged += Setup;
    }

    private void Setup(WindowStateEventType type)
    {
        if (isInit) return;
        isInit = true;

        windowController.windowSize = new Vector2(Screen.currentResolution.width, 250);

        Vector2 usableScreen = GetUsableDesktopSize();
        windowController.windowPosition = new Vector2(0, Screen.currentResolution.height - usableScreen.y);


        //Debug.Log("Screen: " + windowController.windowSize);
        //Debug.Log("Screen pos: " + windowController.windowPosition);
        //Debug.Log("taskbar size: " + usableScreen.y);
    }

    private Vector2 GetUsableDesktopSize()
    {
#if UNITY_STANDALONE_WIN
        Rect taskbar = UtilsWindowsSO.GetTaskbarRect();
        return new Vector2(Screen.currentResolution.width, taskbar.y);

#elif UNITY_STANDALONE_OSX
        return UtilsMacOS.GetVisibleFrameSize();

#else
        Rect taskbar = UtilsWindowsSO.GetTaskbarRect();
        return new Vector2(Screen.currentResolution.width, taskbar.y);
#endif
    }
}
