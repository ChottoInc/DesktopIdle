using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabJobNecromancer : UITabWindow
{
    [Space(10)]
    [SerializeField] UITabPlayerJob panelJob;



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
        // add rituals to update their ui if now the abilities are maxed out and revel themself

        /*
        foreach (var uiSpell in mageSpells)
        {
            uiSpell.Refresh();
        }

        slot1.Setup(PlayerManager.Instance.PlayerMageData.EquippedSlot1Spell);
        slot2.Setup(PlayerManager.Instance.PlayerMageData.EquippedSlot2Spell);
        slot3.Setup(PlayerManager.Instance.PlayerMageData.EquippedSlot3Spell);
        slot4.Setup(PlayerManager.Instance.PlayerMageData.EquippedSlot4Spell);*/
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
