using UnityEngine;
using UnityEngine.UI;

public class UITabJobMage : UITabWindow
{
    [Space(10)]
    [SerializeField] UITabPlayerJob panelJob;

    [Header("Spells")]
    [SerializeField] ScrollRect scrollSpells;
    [SerializeField] UISpellInfoPrefab[] mageSpells;

    public ScrollRect ScollSpells => scrollSpells;

    [Header("Slots")]
    [SerializeField] UISlotSpellPrefab slot1;
    [SerializeField] UISlotSpellPrefab slot2;
    [SerializeField] UISlotSpellPrefab slot3;
    [SerializeField] UISlotSpellPrefab slot4;

    private PlayerMage _player;



    public PlayerMage Player => _player;


    public override void Open()
    {
        base.Open();
        
        if (_player == null)
        {
            _player = FindFirstObjectByType<PlayerMage>();
        }
        
        panelJob.ChangeCurrentTab(this, UtilsPlayer.PlayerJob.Mage);

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
        foreach (var uiSpell in mageSpells)
        {
            uiSpell.Refresh();
        }

        slot1.Setup(PlayerManager.Instance.PlayerMageData.EquippedSlot1Spell);
        slot2.Setup(PlayerManager.Instance.PlayerMageData.EquippedSlot2Spell);
        slot3.Setup(PlayerManager.Instance.PlayerMageData.EquippedSlot3Spell);
        slot4.Setup(PlayerManager.Instance.PlayerMageData.EquippedSlot4Spell);
    }


    public void OnSpellSelected()
    {
        if (_player != null)
        {
            panelJob.OnButtonClose(false);

            _player.RefreshSpell();
        }
        else
        {
            LastSceneSettings settings = new LastSceneSettings();
            settings.lastSceneName = "MageScene";
            settings.lastSceneType = SceneLoaderManager.SceneType.Mage;

            SceneLoaderManager.Instance.LoadScene(settings);
        }
    }
}
