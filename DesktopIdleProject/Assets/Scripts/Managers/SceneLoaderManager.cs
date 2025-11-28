using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    public enum SceneType { CombatMap }


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


    /// <summary>
    /// Used by initialier manager, and does not fade out
    /// </summary>
    public void LoadFirstScene(LastSceneSettings settings)
    {
        SettingsManager.Instance.SetSceneSettings(settings);
        SceneManager.LoadScene(settings.lastSceneName);
    }

    public void LoadScene(LastSceneSettings settings)
    {
        // if the time was stopped before changing scene, resume it
        UtilsTime.Resume();

        // disable scene barrier after loading
        currentSceneBarrier = FindFirstObjectByType<UIBarrier>();
        if(currentSceneBarrier != null)
        {
            currentSceneBarrier.EnableBarrier(false);
        }

        // handle current scene hide objects

        switch (SettingsManager.Instance.LastSceneSettings.lastSceneType)
        {
            default: Debug.Log("Current scene type isn't allowed"); break;
            case SceneType.CombatMap: CombatManager.Instance.HandleSwitchScene(); break;
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
            return;
        }

        StartCoroutine(CoFadeIn());

        LastSceneSettings settings = SettingsManager.Instance.LastSceneSettings;

        if (settings.lastSceneType == SceneType.CombatMap)
        {
            CombatManager.Instance.Setup(UtilsCombatMap.GetMapById(settings.lastCombatMapId));
            FindFirstObjectByType<UIManager>().Setup();
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