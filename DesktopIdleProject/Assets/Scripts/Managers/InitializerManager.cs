using Kirurobo;
using UnityEngine;

using static Kirurobo.UniWindowController;

public class InitializerManager : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] UniWindowController windowController;

    [Space(10)]
    [SerializeField] float offsetBound = 200f;

    [Header("UI")]
    [SerializeField] UIManagerCombatMap uiManagerCombatMap;

    private bool isInit;

    public static InitializerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        windowController.OnStateChanged += Setup;
    }

    private void Setup(WindowStateEventType type)
    {
        if (isInit) return;
        isInit = true;

        windowController.windowSize = new Vector2(Screen.currentResolution.width, 250);

        Vector2 usableScreen = GetUsableDesktopSize();
        windowController.windowPosition = new Vector2(0, Screen.currentResolution.height - usableScreen.y);


        HandleOtherSetups();

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




    private void HandleOtherSetups()
    {
        CombatManager.Instance.Setup();
        uiManagerCombatMap.Setup();
    }

    public static float GetScreenWidth()
    {
        return Screen.currentResolution.width;
    }

    public float GetScreenOffsetBound()
    {
        return offsetBound;
    }
}
