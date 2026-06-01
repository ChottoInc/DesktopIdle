using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPanelHome : MonoBehaviour
{
    [SerializeField] CustomSteamManager steamManager;
    [SerializeField] UISelfAppear contentAppear;

    [Header("Buttons")]
    [SerializeField] Button buttonContinue;
    [SerializeField] Button buttonNew;
    [SerializeField] Button buttonQuit;

    [Space(10)]
    [SerializeField] TMP_Text textContinue;
    [SerializeField] Color disableColor;

    [Space(10)]
    [SerializeField] Transform messageNewGamePosition;

    [Space(10)]
    [SerializeField] UIPanelAdd panelAdd;

    private bool isInit;

    private bool isChangingScene;

    private void Awake()
    {
        steamManager.OnCheckedSettings += Init;
    }

    private void OnDestroy()
    {
        steamManager.OnCheckedSettings -= Init;
    }

    public void Start()
    {
        buttonContinue.interactable = false;
        buttonNew.interactable = false;
        buttonQuit.interactable = false;
    }

    private void Update()
    {
        if (isInit) return;

        if (InitializerManager.Instance.HasCheckFiles)
        {
            if (!SettingsManager.Instance.IsSteamPlatform)
            {
                Init();
            }
        }
    }

    private void Init()
    {
        Setup();

        isInit = true;
    }

    public void Setup()
    {
        buttonContinue.interactable = InitializerManager.Instance.HasSaveFile;

        if (buttonContinue.interactable)
        {
            textContinue.color = Color.white;
        }
        else
        {
            textContinue.color = disableColor;
        }

        buttonNew.interactable = true;
        buttonQuit.interactable = true;

        contentAppear.Appear();

        //Debug.Log(SettingsManager.Instance.LastSceneSettings.lastSceneName);
    }

    public void OnButtonContinue()
    {
        if (isChangingScene) return;

        isChangingScene = true;
        SceneLoaderManager.Instance.LoadScene(SettingsManager.Instance.LastSceneSettings);
    }

    public async void OnButtonNew()
    {
        if (isChangingScene) return;

        if (!InitializerManager.Instance.HasSaveFile)
        {
            // last scene setting is initalized by default from initializer
            InitializerManager.Instance.SetHasSaveFile();

            SceneLoaderManager.Instance.LoadScene(SettingsManager.Instance.LastSceneSettings);

            isChangingScene = true;
        }
        else
        {
            string question = UtilsText.AllTextDictionary[UtilsText.text_yesno_newgame];

            TooltipManagerData tooltipData = new TooltipManagerData();
            tooltipData.idTooltip = UITooltipManager.ID_SHOW_YESNO;
            tooltipData.text = question;

            bool confirm = await UITooltipManager.Instance.ShowPanelYesNoCallback(tooltipData, messageNewGamePosition.position, true);

            if (confirm)
            {
                // erase and recreate default
                InitializerManager.Instance.EraseAllSaves();
                InitializerManager.Instance.HandleSaves();

                // set var that has files
                InitializerManager.Instance.SetHasSaveFile();

                // call steammanager to get the default lang
                if (SettingsManager.Instance.IsSteamPlatform)
                {
                    steamManager.CheckDefaultLang();
                }

                // load first scene
                SceneLoaderManager.Instance.LoadScene(SettingsManager.Instance.LastSceneSettings);

                isChangingScene = true;
            }
        }
    }

    public void OnButtonQuit()
    {
        // todo, add message to close?
        Application.Quit();
    }

    public void OnButtonAdd()
    {
        Debug.Log("culo");
        if (!SettingsManager.Instance.AreCheatsEnabled) return;

        panelAdd.gameObject.SetActive(true);
    }
}
