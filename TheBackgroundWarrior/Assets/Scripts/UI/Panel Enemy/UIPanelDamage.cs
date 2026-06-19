using UnityEngine;

public class UIPanelDamage : UIBasePanelFloating
{
    [SerializeField] Enemy enemy;

    protected override void Awake()
    {
        enemy.OnTakeDamage += ShowValue;
    }

    protected override void OnDestroy()
    {
        enemy.OnTakeDamage -= ShowValue;
    }
}
