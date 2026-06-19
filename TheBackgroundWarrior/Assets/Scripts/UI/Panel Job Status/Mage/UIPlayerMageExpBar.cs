using UnityEngine;

public class UIPlayerMageExpBar : GenericBar
{
    [Space(10)]
    [SerializeField] PlayerMage player;

    private void OnDestroy()
    {
        if (player.PlayerData != null)
            player.PlayerData.OnAddedExp -= UpdateBar;
    }

    public void Setup()
    {
        player.PlayerData.OnAddedExp += UpdateBar;
        UpdateBar();
    }

    private void UpdateBar()
    {
        SetMaxValue(player.PlayerData.ExpToNextLevel);
        SetCurrentValue(player.PlayerData.CurrentExp);
    }
}
