using System.Collections;
using UnityEngine;

public class NecromancerFightingSpot : MonoBehaviour
{
    [SerializeField] PlayerNecromancer _player;

    [Space(10)]
    [SerializeField] NecromancerFighter _fighter1;
    [SerializeField] NecromancerFighter _fighter2;

    [Space(10)]
    [SerializeField] float _timerResetFight = 4f;

    public bool IsInitialized { get; private set; }


    private void OnDestroy()
    {
        _fighter1.OnPerformedAttack -= Fighter1Performer;
        _fighter2.OnPerformedAttack -= Fighter2Performer;
    }


    private void Awake()
    {
        _fighter1.OnPerformedAttack += Fighter1Performer;
        _fighter2.OnPerformedAttack += Fighter2Performer;
    }


    public void Initialize()
    {
        // show them in scene, sprite is handled in script
        _fighter1.gameObject.SetActive(true);
        _fighter2.gameObject.SetActive(true);

        _fighter1.Initialize();
        _fighter2.Initialize();

        

        IsInitialized = true;
    }

    private void Fighter1Performer(int damage)
    {
        _fighter2.Hit(damage);

        if (_fighter2.IsDead)
        {
            _fighter1.IdleAnimation();
            _fighter2.DeathAnimation();

            StartCoroutine(CoResetFight());

            _player.GiveExp();
        }
    }

    private void Fighter2Performer(int damage)
    {
        _fighter1.Hit(damage);

        if (_fighter1.IsDead)
        {
            _fighter2.IdleAnimation();
            _fighter1.DeathAnimation();

            StartCoroutine(CoResetFight());

            _player.GiveExp();
        }
    }

    private IEnumerator CoResetFight()
    {
        yield return new WaitForSeconds(_timerResetFight);

        _player.SummonAnimation();

        _fighter1.Initialize();
        _fighter2.Initialize();
    }
}
