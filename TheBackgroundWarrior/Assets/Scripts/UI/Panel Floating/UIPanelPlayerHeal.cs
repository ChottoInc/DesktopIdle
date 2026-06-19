using UnityEngine;

public class UIPanelPlayerHeal : UIBasePanelFloating
{
    [SerializeField] PlayerFight player;

    protected override void Awake()
    {
        player.OnHeal += ShowValue;
    }

    protected override void OnDestroy()
    {
        player.OnHeal -= ShowValue;
    }
}
