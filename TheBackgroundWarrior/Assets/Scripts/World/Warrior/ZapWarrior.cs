using UnityEngine;

public class ZapWarrior : SpellWarrior
{
    [Space(10)]
    [SerializeField] LayerMask enemyMask;

    private int currentBounces;

    protected override void MakeEffect()
    {
        if(_targetEnemy == null) { Debug.Log("target enemy is null");  _canDestroy = true; return; }

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

    public override void EndAnimation()
    {
        // check for bounces and create new zap towards new enemy
        CheckBounce();

        // request destroy
        base.EndAnimation();
    }

    private void CheckBounce()
    {
        if (currentBounces < _spellData.Bounces)
        {
            Enemy next = GetNearbyEnemy();

            if(next != null)
                SpawnSpell(gameObject, _spellData, next);
        }
    }

    private Enemy GetNearbyEnemy()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _spellData.Radius, enemyMask);

        return GetFirstNotSame(hits);
    }

    private Enemy GetFirstNotSame(Collider2D[] hits)
    {
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy.EnemyIndex != _targetEnemy.EnemyIndex)
                return enemy;
        }
        return null;
    }

    private void SpawnSpell(GameObject prefab, SpellData spellData, Enemy enemy)
    {
        GameObject spawned = Instantiate(prefab, transform.position, Quaternion.identity);

        if (spawned.TryGetComponent(out SpellWarrior spellWarrior))
        {
            // set data
            spellWarrior.SetData(spellData);

            // set start position next zap
            spellWarrior.SetPositions(transform.position, new Vector2(enemy.transform.position.x, transform.position.y));

            // set next target
            spellWarrior.SetTargetEnemy(enemy);

            spellWarrior.GetComponent<ZapWarrior>().SetBounce(++currentBounces);

            spellWarrior.Perform();
        }
    }

    public void SetBounce(int val)
    {
        currentBounces = val;
    }
}
