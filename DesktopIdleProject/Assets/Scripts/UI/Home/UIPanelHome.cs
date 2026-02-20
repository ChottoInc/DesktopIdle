using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPanelHome : MonoBehaviour
{
    [SerializeField] Button buttonContinue;
    [SerializeField] Button buttonNew;
    [SerializeField] Button buttonQuit;

    [Space(10)]
    [SerializeField] Transform messageNewGamePosition;

    private bool isInit;


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
            Setup();
            isInit = true;
        }
    }

    private void Setup()
    {
        buttonContinue.interactable = InitializerManager.Instance.HasSaveFile;

        buttonNew.interactable = true;
        buttonQuit.interactable = true;

        //Debug.Log(SettingsManager.Instance.LastSceneSettings.lastSceneName);
    }

    public void OnButtonContinue()
    {
        Debug.Log("Continue Button");
        SceneLoaderManager.Instance.LoadScene(SettingsManager.Instance.LastSceneSettings);
    }

    public async void OnButtonNew()
    {
        if(!InitializerManager.Instance.HasSaveFile)
        {
            // last scene setting is initalized by default from initializer
            InitializerManager.Instance.SetHasSaveFile();

            SceneLoaderManager.Instance.LoadScene(SettingsManager.Instance.LastSceneSettings);
        }
        else
        {
            string question = $"You already have an adventure in progress, starting a new game will erase your current save files.\nAre you sure you want to continue?";

            TooltipManagerData tooltipData = new TooltipManagerData();
            tooltipData.idTooltip = UITooltipManager.ID_SHOW_YESNO;
            tooltipData.text = question;

            bool confirm = await UITooltipManager.Instance.ShowPanelYesNoCallback(tooltipData, messageNewGamePosition.position, true);

            if (confirm)
            {
                // erase and recreate default
                InitializerManager.Instance.EraseAllSaves();
                InitializerManager.Instance.HandleSaves();

                // switch to first scene
                InitializerManager.Instance.SetHasSaveFile();
                SceneLoaderManager.Instance.LoadScene(SettingsManager.Instance.LastSceneSettings);
            }
        }
    }

    public void OnButtonQuit()
    {
        // todo, add message to close?
        Application.Quit();
    }

    public void OnButtonTest()
    {
        SceneManager.LoadScene("FisherScene");
    }
}
