using UnityEngine;

public class UITabJobNecromancer : UITabWindow
{
    [Space(10)]
    [SerializeField] UITabPlayerJob panelJob;

    [Space(10)]
    [SerializeField] UIRitualPrefab[] _ritualPrefabs;



    private PlayerNecromancer _player;


    public PlayerNecromancer Player => _player;

    public override void Open()
    {
        base.Open();

        if (_player == null)
        {
            _player = FindFirstObjectByType<PlayerNecromancer>();
        }

        panelJob.ChangeCurrentTab(this, UtilsPlayer.PlayerJob.Necromancer);

        // refrresh all ritual to update their texts
        Refresh();
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        panelJob.ChangeCurrentTab(null, UtilsPlayer.PlayerJob.None);
    }

    private void Refresh()
    {
        foreach (var uiRitual in _ritualPrefabs)
        {
            uiRitual.Refresh();
        }
    }


    public void OnButtonChant()
    {
        if (_player != null)
        {
            panelJob.OnButtonClose(false);
        }

        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "NecromancerScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Necromancer;

        SceneLoaderManager.Instance.LoadScene(settings);
    }
}
