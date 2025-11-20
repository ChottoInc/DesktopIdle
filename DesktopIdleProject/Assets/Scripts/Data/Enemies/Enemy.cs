using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldownIdle = 1.5f;

    [Space(10)]
    [SerializeField] Animator animator;


    [Header("Death")]
    [SerializeField] ParticleSystem deathVFX;


    private float deathVFXDuration;
    private float timerDeathVFX;



    private EnemyData enemyData;

    private int enemyIndex;


    // --------- MOVEMENT VARS

    private Vector3 startScale;

    private float currentTarget;

    private bool isIdling;
    private float timerIdle;

    private Rigidbody2D rb;

    // --------- ATTACK VARS

    private bool isAttacking;
    private float CooldownAttack => 1f / enemyData.CurrentAtkSpd;
    private float timerAttack;

    public event Action OnPerformAttack;



    // ------- DEATH

    private bool isDeathVFXPlaying;




    public EnemyData EnemyData => enemyData;

    public bool IsDead => enemyData.CurrentHp <= 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        deathVFXDuration = deathVFX.main.duration;

        startScale = transform.localScale;

        GenerateNewTarget();
    }


    private void Update()
    {
        if (isAttacking)
        {
            CheckAttack();
        }

        if (isDeathVFXPlaying)
        {
            CheckDeathVFX();
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f && !isIdling)
        {
            // get target dir
            Vector2 dir = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move with rb
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

            CheckFlip();
        }
        else
        {
            // handles idling timer before move again
            if (!isIdling)
            {
                timerIdle = cooldownIdle;
                isIdling = true;
            }
            else
            {
                if (timerIdle <= 0)
                {
                    GenerateNewTarget();
                    isIdling = false;
                }
                else
                {
                    timerIdle -= Time.fixedDeltaTime;
                }
            }
        }
    }

    private void CheckFlip()
    {
        // check sprite flip
        float vx = rb.velocity.x;
        if (vx > 0.01f && faceRight)
        {
            transform.localScale = startScale;
        }
        else if (vx > 0.01f && !faceRight)
        {
            transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (vx < -0.01f && faceRight)
        {
            transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (vx < -0.01f && !faceRight)
        {
            transform.localScale = startScale;
        }
    }

    private void GenerateNewTarget()
    {
        currentTarget = UnityEngine.Random.Range(InitializerManager.Instance.GetScreenOffsetBound(), InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound());
        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(currentTarget, 0)).x;
    }

    private void CheckAttack()
    {
        if (timerAttack <= 0)
        {
            OnPerformAttack?.Invoke();
            timerAttack = CooldownAttack;
        }
        else
        {
            timerAttack -= Time.deltaTime;
        }
    }

    private void CheckDeathVFX()
    {
        if (timerDeathVFX <= 0)
        {
            isDeathVFXPlaying = false;
            HideAfterDeath();
        }
        else
        {
            timerDeathVFX -= Time.deltaTime;
        }
    }

    public void Setup(EnemyData enemyData, int index)
    {
        this.enemyData = enemyData;

        enemyIndex = index;

        spriteRenderer.sortingOrder = enemyIndex;
    }

    private void HideSprite(bool hide)
    {
        // save initial color
        Color spriteColor = spriteRenderer.color;

        if(hide)
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0);
        else
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);
    }


    public void SetAttacking(bool isAttacking)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        this.isAttacking = isAttacking;

        // reset attacking when change enemy basically
        timerAttack = CooldownAttack;

        animator.SetBool("isAttacking", isAttacking);
    }

    public void PlayDeath(bool setDead)
    {
        if (setDead)
            enemyData.SetDead();

        HideSprite(true);

        // play vfx
        deathVFX.Play();
        timerDeathVFX = deathVFXDuration;
        isDeathVFXPlaying = true;
    }

    private void HideAfterDeath()
    {
        deathVFX.Stop();
        Die();
    }

    public void OnSpawn()
    {
        HideSprite(false);
    }

    public void OnDespawn()
    {
        
    }

    public void Die() 
    {
        StageManager.Instance.RemoveFromCurrentEnemiesList(this);
        PoolManager.Instance.Return(gameObject, "Enemy");
    }




    public override bool Equals(object other)
    {
        Enemy otherEnemy = other as Enemy;
        return enemyIndex == otherEnemy.enemyIndex;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
