using TMPro;
using UnityEngine;

public class UIPanelAverageHookTime : MonoBehaviour
{
    [SerializeField] TMP_Text textAverage;

    [SerializeField] PlayerFisher player;

    private bool isInitialized;

    private void OnDestroy()
    {
        player.OnStatChange -= CheckStatChange;
        SettingsManager.Instance.OnLanguageChange -= UpdateUI;
    }

    private void Awake()
    {
        player.OnStatChange += CheckStatChange;
        SettingsManager.Instance.OnLanguageChange += UpdateUI;
    }

    private void Update()
    {
        if (isInitialized) return;

        if(player.PlayerData != null)
        {
            UpdateUI();

            isInitialized = true;
        }

    }

    private void UpdateUI()
    {
        float average = FishSpawnManager.Instance.AverageHookTime;
        textAverage.text = string.Format(
            UtilsText.AllText[UtilsText.text_job_fisher_waittime], 
            Mathf.FloorToInt(average / 60f),
            Mathf.FloorToInt(average % 60f));
    }

    private void CheckStatChange(int id, int amount)
    {
        if(id == UtilsPlayer.ID_FISHER_CALMNESS)
        {
            UpdateUI();
        }
    }
}
