using System;
using System.Collections;
using UnityEngine;

public class PlayerFight : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip walkClip;
    [SerializeField] AnimationClip attackClip;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;


    private const int ATTACK_ANIMATION_FRAMES = 7;
    private const int ATTACK_FRAME_INDEX = 3;

    private float startingAttackSpeedAnimationDuration;



    [Header("Combat")]
    [SerializeField] Transform checkEnemyPoint;
    [SerializeField] LayerMask enemyLayer;

    [Space(10)]
    [SerializeField] GameObject swordHitVFXPrefab;

    [Space(10)]
    [SerializeField] GenericBar hpBar;

    [Header("Death")]
    [SerializeField] float timerResetAfterDeath = 2f;


    private PlayerFightData playerData;

    // ------ MOVEMENT VARS

    private Vector3 startScale;

    private float currentTarget;

    private bool isIdling;

    private bool dirLeft;

    private Rigidbody2D rb;

    private float CurrentSpeed => speed *
        PlayerManager.Instance.FisherQuickSeriesMultiplier;

    // ------ ATTACK VARS

    private bool isEnemyDetected;

    private bool isAttacking;
    private float CooldownAttack => 1f / playerData.CurrentAtkSpd;
    private float timerAttack;



    public event Action OnPerformAttack;


    public event Action<int, int> OnStatChange;
    public event Action<int> OnAddMap;


    public event Action OnResetAfterDeath;




    public PlayerFightData PlayerData => playerData;

    public bool IsDead => playerData.CurrentHp <= 0;





    private void OnDestroy()
    {
        if(playerData != null)
        {
            playerData.OnHpChange -= UpdateHpBarUI;
            playerData.OnLevelUp -= SaveFightData;

            playerData.OnStatChange -= OnStatChangeFight;
            playerData.OnAddMap -= OnAddMapFight;
        }

        OnPerformAttack -= PlaySwordHit;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get default speed for animator walk
        startingAttackSpeedAnimationDuration = attackClip.length;

        OnPerformAttack += PlaySwordHit;
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
        if (!isAttacking && !IsDead)
        {
            HandleMovement();

            CheckForEnemy();
        }
    }

    private void CheckAttack()
    {
        if (timerAttack <= 0)
        {
            //Debug.Log("Weapon mult: " + PlayerManager.Instance.WeaponMinerMultiplier);
            //Debug.Log("Weapon level: " + PlayerManager.Instance.PlayerMinerData.WeaponLevel);
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
        if (isEnemyDetected) return;

        if (CheckEnemyAtPoint(checkEnemyPoint.position, 0.5f, enemyLayer, out Collider2D hit))
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if(!enemy.IsDead && !IsDead)
            {
                isEnemyDetected = true;
                CombatManager.Instance.StartFight(enemy);
            }
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
            rb.velocity = new Vector2(dir.x * CurrentSpeed, rb.velocity.y);

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
            playerData.OnLevelUp += SaveFightData;

            playerData.OnStatChange += OnStatChangeFight;
            playerData.OnAddMap += OnAddMapFight;
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

        //reset detection rocks for update
        if (!isAttacking)
        {
            isEnemyDetected = false;

            // Stop sword hit VFX
            StopAllCoroutines();
        }
        else
        {
            float attackSpeedMultiplier = startingAttackSpeedAnimationDuration / CooldownAttack;

            // Set the animator speed accordingly to Atk Spd
            animator.SetFloat("AttackSpeedMultiplier", attackSpeedMultiplier);

            timerAttack = 0;
        }

        animator.SetBool("isAttacking", isAttacking);
    }

    private void PlaySwordHit()
    {
        // Time the sowrd VFX with the animation
        float hitNormalizedTime = (float)ATTACK_FRAME_INDEX / (float)ATTACK_ANIMATION_FRAMES; // Tweak that to make the hit appear sooner or later
        float timer = CooldownAttack * hitNormalizedTime;

        StartCoroutine(CoPlaySwordHit(timer));
    }

    private IEnumerator CoPlaySwordHit(float timer)
    {
        yield return new WaitForSeconds(timer);

        Enemy currentEnemy = CombatManager.Instance.CurrentEnemy;

        if (currentEnemy != null)
        {
            GameObject hitVFX = Instantiate(swordHitVFXPrefab, CombatManager.Instance.CurrentEnemy.transform.position, Quaternion.identity);
            hitVFX.transform.parent = null;

            currentEnemy.UpdateDamageUI();
        }
    }

    public bool CheckEnemyAtPoint(Vector2 point, float radius, LayerMask enemyMask, out Collider2D hitEnemy)
    {
        hitEnemy = Physics2D.OverlapCircle(point, radius, enemyMask);
        return hitEnemy != null;
    }

    public void SetDeath(bool isDead)
    {
        if(isDead)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);

            animator.SetBool("isDead", true);

            StartCoroutine(CoResetAfterDeath());
        }
        else
        {
            animator.SetBool("isDead", false);
        }
    }

    private IEnumerator CoResetAfterDeath()
    {
        yield return new WaitForSeconds(timerResetAfterDeath);

        OnResetAfterDeath?.Invoke();
    }

    #region SAVE

    public void SaveFightData()
    {
        PlayerManager.Instance.UpdateFightData(playerData);
        PlayerManager.Instance.SaveFightData();
    }

    #endregion

    #region HANDLE EVENTS FROM FIGHTER DATA

    private void OnStatChangeFight(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    private void OnAddMapFight(int id)
    {
        OnAddMap?.Invoke(id);
    }

    #endregion
}
