using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldownIdle = 1.5f;


    [Header("Combat")]
    [SerializeField] Transform checkEnemyPoint;
    [SerializeField] LayerMask enemyLayer;

    [Space(10)]
    [SerializeField] GenericBar hpBar;


    private PlayerFightData playerData;

    // ------ MOVEMENT VARS

    private Vector3 startScale;

    private float currentTarget;

    private bool isIdling;
    private float timerIdle;

    private bool dirLeft;

    private Rigidbody2D rb;

    // ------ ATTACK VARS

    private bool isAttacking;
    private float CooldownAttack => 1f / playerData.CurrentAtkSpd;
    private float timerAttack;



    public event Action OnPerformAttack;




    public PlayerFightData PlayerData => playerData;

    public bool IsDead => playerData.CurrentHp <= 0;


    private void OnDestroy()
    {
        if(playerData != null)
        {
            playerData.OnHpChange -= UpdateHpBarUI;
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startScale = spriteRenderer.transform.localScale;

        GenerateNewTarget();
    }

    private void Update()
    {
        if (isAttacking)
        {
            CheckAttack();
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            HandleMovement();

            CheckForEnemy();
        }
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

    private void CheckForEnemy()
    {
        if (CheckEnemyAtPoint(checkEnemyPoint.position, 0.3f, enemyLayer, out Collider2D hit))
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if(!enemy.IsDead && !IsDead)
                CombatManager.Instance.StartFight(enemy);
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
            // reverse direction if destination reached
            GenerateNewTarget();
        }
    }

    private void CheckFlip()
    {
        // check sprite flip
        float vx = rb.velocity.x;
        if (vx > 0.01f && faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
        else if (vx > 0.01f && !faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (vx < -0.01f && faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if(vx < -0.01f && !faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
    }

    private void GenerateNewTarget()
    {
        dirLeft = !dirLeft;

        if (dirLeft)
        {
            currentTarget = InitializerManager.Instance.GetScreenOffsetBound();
        }
        else
        {
            currentTarget = InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound();
        }

        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(currentTarget, 0)).x;
    }



    public void Setup(PlayerFightData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnHpChange += UpdateHpBarUI;
        }

        hpBar.Setup(playerData.MaxHp, playerData.CurrentHp);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateHpBarUI()
    {
        hpBar.SetCurrentValue(playerData.CurrentHp);
    }

    public void SetAttacking(bool isAttacking)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        this.isAttacking = isAttacking;

        animator.SetBool("isAttacking", isAttacking);
    }

    public bool CheckEnemyAtPoint(Vector2 point, float radius, LayerMask enemyMask, out Collider2D hitEnemy)
    {
        hitEnemy = Physics2D.OverlapCircle(point, radius, enemyMask);
        return hitEnemy != null;
    }
}
