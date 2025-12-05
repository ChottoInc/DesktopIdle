using Kirurobo;
using System.IO;
using UnityEngine;

using static Kirurobo.UniWindowController;

public class InitializerManager : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] UniWindowController windowController;

    [Space(10)]
    [SerializeField] float offsetBound = 200f;

    [Header("Scene Loader")]
    [SerializeField] SceneLoaderManager sceneLoaderManager;




    private IDataService jsonService = new JsonDataService();



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

        /*
         * need to check which scene currently in when loading up, and get the correct player data to feed to the manager
         * */

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
        // utils setups
        UtilsItem.Initialize();
        UtilsCombatMap.Initialize();
        UtilsGather.Initialize();

        // load files
        HandleSaves();

        // call loader scene setup - set material
        sceneLoaderManager.Setup();

        // check save for last scene - loading scene manager should handle the alpha
        sceneLoaderManager.LoadFirstScene(SettingsManager.Instance.LastSceneSettings);
    }

    private void HandleSaves()
    {
        string persistent = Application.persistentDataPath + "/";

        // Create folder if never opened
        if (!Directory.Exists(persistent + UtilsSave.ROOT_FOLDER))
        {
            Directory.CreateDirectory(persistent + UtilsSave.ROOT_FOLDER);

            /*
             * need
             * player folder
             * settings folder
             * */

            Directory.CreateDirectory(persistent + UtilsSave.GetPlayerFolder());
            Directory.CreateDirectory(persistent + UtilsSave.GetSettingsFolder());
            Directory.CreateDirectory(persistent + UtilsSave.GetCombatMapsFolder());
        }


        SettingsManager.Instance.Setup(jsonService);
        PlayerManager.Instance.Setup(jsonService);
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
