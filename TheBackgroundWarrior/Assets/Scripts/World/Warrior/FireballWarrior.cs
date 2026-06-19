using UnityEngine;

public class FireballWarrior : SpellWarrior
{
    protected override void MakeEffect()
    {

        if (_targetEnemy == null) { Debug.Log("target enemy is null"); _canDestroy = true; return; }

        if (_targetEnemy.EnemyData == null) { Debug.Log("enemy data is null"); _canDestroy = true; return; }

        float damage = _targetEnemy.EnemyData.MaxHp * _spellData.PercDamage;

        if (!_targetEnemy.IsDead)
        {
            _targetEnemy.EnemyData.TakeDamageFromSpell(damage);

            if (_targetEnemy.IsDead && !_targetEnemy.IsAttacking)
            {
                if (CombatManager.Instance != null)
                {
                    CombatManager.Instance.HandleEnemyDeath(_targetEnemy);
                }
            }
        }

        _canDestroy = true;
    }
}
