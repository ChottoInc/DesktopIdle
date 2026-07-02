using UnityEngine;

public class UIPlayerExpBar : GenericBar
{
    [Space(10)]
    [SerializeField] Player player;

    private void OnDestroy()
    {
        if (player.GetPlayerData() != null)
            player.GetPlayerData().OnAddedExp -= UpdateBar;
    }

    public void Setup()
    {
        player.GetPlayerData().OnAddedExp += UpdateBar;
        UpdateBar();
    }

    private void UpdateBar()
    {
        SetMaxValue(player.GetExpToNextLevel());
        SetCurrentValue(player.GetCurrenExp());
    }
}
