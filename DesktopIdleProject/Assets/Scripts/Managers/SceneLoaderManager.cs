using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    public enum SceneType { Home, CombatMap, Miner }


    [SerializeField] Material fadeMaterial;

    [Space(10)]
    [SerializeField] float onFadeInStart = 45f;
    [SerializeField] float onFadeInEnd = 60f;

    [Space(10)]
    [SerializeField] float onFadeOutStart = -60f;
    [SerializeField] float onFadeOutEnd = -45f;

    [Space(10)]
    [SerializeField] float fadeTime = 3f;

    private bool isInit;
    private bool isSetup;

    private int fadeStart = Shader.PropertyToID("_FadeStart");
    private int fadeEnd = Shader.PropertyToID("_FadeEnd");


    private UIBarrier currentSceneBarrier;

    public static SceneLoaderManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(LastSceneSettings settings)
    {
        // if the time was stopped before changing scene, resume it
        UtilsTime.Resume();

        // handle current scene hide objects

        if(SceneManager.GetActiveScene().name == "HomeScene")
        {
            HomeWorldManager.Instance.HandleSwitchScene();
        }
        else
        {
            switch (SettingsManager.Instance.LastSceneSettings.lastSceneType)
            {
                default: Debug.Log("Current scene type isn't allowed"); break;
                case SceneType.CombatMap: CombatManager.Instance.HandleSwitchScene(); break;
                case SceneType.Miner: SmashManager.Instance.HandleSwitchScene(); break;
            }
        }

        StartCoroutine(CoChangeScene(settings));
    }



    private IEnumerator CoChangeScene(LastSceneSettings settings)
    {
        // enable scene barrier before loading
        currentSceneBarrier = FindFirstObjectByType<UIBarrier>();
        if (currentSceneBarrier != null)
        {
            currentSceneBarrier.EnableBarrier(true);
        }


        StartCoroutine(CoFadeOut());

        yield return new WaitForSecondsRealtime(fadeTime + 0.5f);

        SettingsManager.Instance.SetSceneSettings(settings);
        SceneManager.LoadScene(settings.lastSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isInit)
        {
            isInit = true;
        }

        // disable scene barrier after loading
        currentSceneBarrier = FindFirstObjectByType<UIBarrier>();
        if (currentSceneBarrier != null)
        {
            currentSceneBarrier.EnableBarrier(false);
        }

        StartCoroutine(CoFadeIn());

        UIManager uiManager = null;

        // if home only
        if (scene.name == "HomeScene")
        {
            HomeWorldManager.Instance.Setup();
            //uiManager = FindFirstObjectByType<UIManager>();
        }
        else
        {
            LastSceneSettings settings = SettingsManager.Instance.LastSceneSettings;

            switch (settings.lastSceneType)
            {
                case SceneType.CombatMap:
                    CombatManager.Instance.Setup(UtilsCombatMap.GetMapById(settings.lastCombatMapId));
                    uiManager = FindFirstObjectByType<UIManager>();
                    break;

                case SceneType.Miner:
                    SmashManager.Instance.Setup();
                    uiManager = FindFirstObjectByType<UIManager>();
                    break;
            }
        }

        if (uiManager != null)
        {
            uiManager.Setup();
        }
    }

    private IEnumerator CoFadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeTime;

            fadeMaterial.SetFloat(fadeStart, Mathf.Lerp(onFadeInStart, onFadeOutStart, t));
            fadeMaterial.SetFloat(fadeEnd, Mathf.Lerp(onFadeInEnd, onFadeOutEnd, t));

            yield return null;
        }

        fadeMaterial.SetFloat(fadeStart, onFadeOutStart);
        fadeMaterial.SetFloat(fadeEnd, onFadeOutEnd);
    }

    private IEnumerator CoFadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeTime;

            fadeMaterial.SetFloat(fadeStart, Mathf.Lerp(onFadeOutStart, onFadeInStart, t));
            fadeMaterial.SetFloat(fadeEnd, Mathf.Lerp(onFadeOutEnd, onFadeInEnd, t));

            yield return null;
        }

        fadeMaterial.SetFloat(fadeStart, onFadeInStart);
        fadeMaterial.SetFloat(fadeEnd, onFadeInEnd);
    }

    public void Setup()
    {
        if (isSetup) return;
        isSetup = true;

        fadeMaterial.SetFloat(fadeStart, onFadeInStart);
        fadeMaterial.SetFloat(fadeEnd, onFadeInEnd);
    }
}


[System.Serializable]
public struct LastSceneSettings
{
    public string lastSceneName;

    public SceneLoaderManager.SceneType lastSceneType;

    // save a file with name as namemap+id, and save the infromation needed, like prestige and reached stage
    // combat map
    public int lastCombatMapId;
}