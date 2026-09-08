using UnityEngine;

public class SummonDeathExplosion : MonoBehaviour
{
    [SerializeField] float _radius = 2f;
    [SerializeField] float _percDamage = 0.5f;

    [SerializeField] LayerMask enemyMask;

    public void ExternalAttack()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _radius, enemyMask);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                float damage = enemy.EnemyData.CurrentHp * _percDamage;

                if (!enemy.IsDead)
                {
                    enemy.EnemyData.TakeDamage(damage);

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
    }

    public void ExternalDestroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
