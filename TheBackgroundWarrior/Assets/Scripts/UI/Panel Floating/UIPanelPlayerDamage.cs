using UnityEngine;

public class UIPanelPlayerDamage : UIBasePanelFloating
{
    [SerializeField] PlayerFight player;

    protected override void Awake()
    {
        player.OnTakeDamage += ShowValue;
    }

    protected override void OnDestroy()
    {
        player.OnTakeDamage -= ShowValue;
    }
}
