using UnityEngine;
using UnityEngine.UI;

public class UIPanelHome : MonoBehaviour
{
    [SerializeField] Button buttonContinue;
    [SerializeField] Button buttonNew;
    [SerializeField] Button buttonQuit;

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

        Debug.Log(SettingsManager.Instance.LastSceneSettings.lastSceneName);
    }

    public void OnButtonContinue()
    {
        Debug.Log("Load");
        SceneLoaderManager.Instance.LoadScene(SettingsManager.Instance.LastSceneSettings);
    }

    public void OnButtonNew()
    {
        // last scene setting is initalized by default from initializer
        // todo, add message if has save file that it will erase the save
        InitializerManager.Instance.SetHasSaveFile();

        SceneLoaderManager.Instance.LoadScene(SettingsManager.Instance.LastSceneSettings);
    }

    public void OnButtonQuit()
    {
        // todo, add message to close?
        Application.Quit();
    }
}
