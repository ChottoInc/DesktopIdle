using UnityEngine;

public class ExplosionWarrior : SpellWarrior
{
    [Space(10)]
    [SerializeField] LayerMask enemyMask;

    protected override void MakeEffect()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _spellData.Radius, enemyMask);

        foreach (var hit in hits)
        {
            if(hit.TryGetComponent(out Enemy enemy))
            {
                float damage = enemy.EnemyData.MaxHp * _spellData.PercDamage;

                if (!enemy.IsDead)
                {
                    enemy.EnemyData.TakeDamageFromSpell(damage);

                    if (enemy.IsDead && !enemy.IsAttacking)
                    {
                        if (CombatManager.Instance != null)
                        {
                            CombatManager.Instance.HandleEnemyDeath(enemy);
                        }
                    }
                }
            }
        }

        _canDestroy = true;
    }
}
