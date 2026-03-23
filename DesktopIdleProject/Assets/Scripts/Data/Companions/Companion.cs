using System;
using System.Collections;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldownIdle = 1.5f;

    [Space(10)]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip attackClip;

    [Space(10)]
    [SerializeField] Transform checkEnemyPoint;
    [SerializeField] LayerMask enemyLayer;


    private CompanionSO tempSOBefriend;


    private CompanionData companionData;


    // --------- MOVEMENT VARS

    private Vector3 startScale;

    private float currentTarget;

    private bool isWalking;

    private bool isIdling;
    private float timerIdle;

    // setup vars
    private bool isGoingToCrop;
    private bool isRandomWalking;

    // disappear vars
    private bool isWalkingAway;

    private Rigidbody2D rb;

    // --------- ATTACK VARS


    private bool canAttack;
    private bool isAttacking;

    private float CooldownAttack => 1f / companionData.CurrentAtkSpd;
    private float timerAttack;

    private bool isEnemyDetected;
    private Enemy currentEnemy;


    public CompanionData CurrentCompanionData => companionData;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startScale = spriteRenderer.transform.localScale;

        StartCoroutine(CoSetCanAttack());
    }


    private void Update()
    {
        CheckAttack();
    }

    private void FixedUpdate()
    {
        if (isGoingToCrop || isRandomWalking || isWalkingAway)
        {
            HandleMovement();
        }
        
        // when in fight scene, check for enemy when not attacking
        if (!isAttacking)
        {
            CheckForEnemy();
        }
    }

    private IEnumerator CoSetCanAttack()
    {
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

    private void HandleMovement()
    {
        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f && !isIdling)
        {
            // if not on target and moving, set to walk animator
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("isWalking", isWalking);
            }

            // get target dir
            Vector2 dir = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move with rb
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

            CheckFlip();
        }
        else
        {
            // if is going to crop, can't be walking away
            if (isGoingToCrop)
            {
                HandleTargetCrop();
            }
            else if (isWalkingAway)
            {
                HandleWalkAway();
            }
            else
            {
                HandleStandardWalk();
            }
        }
    }

    private void HandleTargetCrop()
    {
        // set a different idle and action for farmer

        if (!isIdling)
        {
            // stop the player
            rb.velocity = new Vector2(0f, rb.velocity.y);

            timerIdle = 5f;
            isIdling = true;

            isWalking = false;
            animator.SetBool("isWalking", isWalking);

            //Debug.Log("arrived at crop");
        }
        else
        {
            if (timerIdle <= 0)
            {
                isGoingToCrop = false;

                // take action befriended or not
                bool success = UtilsGeneral.GetRandomSuccessFromValue(PlayerManager.Instance.PlayerFarmerData.CurrentLuck);
                if (success)
                {
                    // TODO: balance exp
                    PlayerManager.Instance.PlayerFarmerData.AddExp(5);
                    PlayerManager.Instance.SaveFarmerData();

                    if (PlayerManager.Instance.PlayerFarmerData.HasCompanion(tempSOBefriend))
                    {
                        // TODO: handle if companion is already befriended, dismantle it, for now give bits?
                    }
                    else
                    {
                        // add to companions
                        PlayerManager.Instance.PlayerFarmerData.AddCompanion(tempSOBefriend);
                        PlayerManager.Instance.SaveFarmerData();
                    }
                    
                    // walk away
                    SetTargetOutsideScreen();

                    Debug.Log("Companion befriended");
                }
                else
                {
                    // set the companion to walk away from the screen
                    SetTargetOutsideScreen();

                    Debug.Log("Companion not befriended");
                }

                isIdling = false;
            }
            else
            {
                timerIdle -= Time.fixedDeltaTime;
            }
        }
    }

    private void HandleWalkAway()
    {
        Destroy(gameObject);
    }

    private void HandleStandardWalk()
    {
        // handles idling timer before move again when random walking
        if (!isIdling)
        {
            // stop the player
            rb.velocity = new Vector2(0f, rb.velocity.y);

            timerIdle = cooldownIdle;
            isIdling = true;

            isWalking = false;
            animator.SetBool("isWalking", isWalking);
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

    private void CheckForEnemy()
    {
        if (!canAttack) return;

        // if already engaged an enemy don't do anything
        if (isEnemyDetected) return;

        // find the enemy
        if (CheckEnemyAtPoint(checkEnemyPoint.position, 0.5f, enemyLayer, out Collider2D hit))
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            // if the enemy is not dead and can actually attack, perform attack
            if (!enemy.IsDead && timerAttack <= 0)
            {
                // set detected enemy and is attacking so the companion doesn't look for another one
                isEnemyDetected = true;
                isAttacking = true;

                // disable walking
                isRandomWalking = false;

                isWalking = false;
                animator.SetBool("isWalking", isWalking);

                // stop the player
                rb.velocity = new Vector2(0f, rb.velocity.y);

                // set the current enemy
                currentEnemy = enemy;

                PerformAttack();
            }
        }
    }

    private void CheckAttack()
    {
        if (timerAttack <= 0) return;

        // keep decreasing the timer for the attack
        timerAttack -= Time.deltaTime;
    }

    private void PerformAttack()
    {
        // animate
        animator.SetTrigger("Attack");
        animator.ResetTrigger("Stop");

        // stop enemy movement to synch animation
        if(currentEnemy != null)
        {
            currentEnemy.SetMove(false);
        }

        // wait and start resets
        StartCoroutine(CoStopAttack());
    }

    /// <summary>
    /// Called from the animation to align animation and damages
    /// </summary>
    public void ExternalAttack()
    {
        // damage enemy once
        currentEnemy.EnemyData.TakeDamage(companionData);

        // make the damage number show
        currentEnemy.UpdateDamageUI();
    }

    private IEnumerator CoStopAttack()
    {
        yield return new WaitForSeconds(attackClip.length);

        // animate stop
        animator.SetTrigger("Stop");
        animator.ResetTrigger("Attack");

        // reset enemy vars
        isEnemyDetected = false;
        isAttacking = false;

        // restart enemy movement
        if (currentEnemy != null)
        {
            currentEnemy.SetMove(true);
        }

        currentEnemy = null;

        // reset attack cooldown
        timerAttack = CooldownAttack;

        // random movement in direction
        GenerateNewTarget();

        // re enable walking
        isRandomWalking = true;

        isWalking = true;
        animator.SetBool("isWalking", isWalking);
    }

    public bool CheckEnemyAtPoint(Vector2 point, float radius, LayerMask enemyMask, out Collider2D hitEnemy)
    {
        hitEnemy = Physics2D.OverlapCircle(point, radius, enemyMask);
        return hitEnemy != null;
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
        else if (vx < -0.01f && !faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
    }

    private void GenerateNewTarget()
    {
        currentTarget = UnityEngine.Random.Range(InitializerManager.Instance.GetScreenOffsetBound(), InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound());
        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(currentTarget, 0)).x;
    }

    public void SetTargetOutsideScreen()
    {
        isRandomWalking = false;

        float x;
        if (UnityEngine.Random.value < 0.5f)
        {
            x = InitializerManager.Instance.GetScreenOffsetBound() - 20f;
        }
        else
        {
            x = InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound() + 20f;
        }
        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(x, 0)).x;

        isWalkingAway = true;
    }

    public void SetupBefriend(CompanionSO so, Vector2 cropPos)
    {
        tempSOBefriend = so;

        // set going to crop
        isGoingToCrop = true;

        // set crop target
        currentTarget = cropPos.x;

        //Debug.Log("Setup befriend");
    }

    public void SetupRandomWalk()
    {
        // set going to crop
        isRandomWalking = true;

        GenerateNewTarget();
    }

    public void SetupFight(CompanionData data)
    {
        // set data from farmer save
        companionData = data;

        // new direction
        GenerateNewTarget();

        isRandomWalking = true;
    }

    private void HideSprite(bool hide)
    {
        // save initial color
        Color spriteColor = spriteRenderer.color;

        if (hide)
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0);
        else
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);
    }
}
