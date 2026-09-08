using System;
using System.Collections;
using UnityEngine;

public class NecromancerFighter : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sRenderer;
    [SerializeField] Animator _animator;

    [Header("Animation settings")]
    [SerializeField] float _timerBeforeAttack = 2.5f;
    [SerializeField] float _maxRandAttackAnimationMultiplier = 1.5f;



    public int MaxHp { get; private set; }
    public int CurrentHp { get; private set; }

    public bool IsDead => CurrentHp <= 0;

    private int _attack;

    private bool _isAppeared;

    public event Action<int> OnPerformedAttack;


    public void Initialize()
    {
        // reset stats
        MaxHp = 100;
        CurrentHp = MaxHp;

        _attack = UnityEngine.Random.Range(8, 15);

        // reset animator speed
        _animator.speed = 1f;

        // appear animation
        if(!_isAppeared)
            _animator.SetTrigger("Summon");
        _isAppeared = true;

        // set delay for attack
        StartCoroutine(CoDelayBeforeAttack());
    }

    private IEnumerator CoDelayBeforeAttack()
    {
        yield return new WaitForSeconds(_timerBeforeAttack);

        // set random speed animator to change things a little
        float randSpeed = UnityEngine.Random.Range(1f, _maxRandAttackAnimationMultiplier);
        _animator.speed = randSpeed;

        // set attakc animation, running until death of one fighter
        _animator.SetTrigger("Attack");
    }

    public void IdleAnimation()
    {
        _animator.SetTrigger("Idle");
    }

    public void DeathAnimation()
    {
        _animator.SetTrigger("Death");
        _isAppeared = false;
    }

    public void ExternalAttack()
    {
        // damage enemy
        OnPerformedAttack?.Invoke(_attack);
    }

    public void Hit(int damage)
    {
        CurrentHp -= damage;
        CurrentHp = Math.Max(0, CurrentHp);
    }
}
