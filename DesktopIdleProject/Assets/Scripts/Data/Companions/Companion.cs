using System;
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

    private bool isAttacking;
    //private float CooldownAttack => 1f / enemyData.CurrentAtkSpd;
    private float timerAttack;

    public event Action OnPerformAttack;



    // ------- VFX


    private SceneLoaderManager.SceneType sceneType;




    public CompanionData CurrentCompanionData => companionData;

    //public bool IsDead => enemyData.CurrentHp <= 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startScale = spriteRenderer.transform.localScale;

        //GenerateNewTarget();
    }


    private void Update()
    {
        if (isAttacking)
        {
            //CheckAttack();
        }
    }

    private void FixedUpdate()
    {
        if (isGoingToCrop || isRandomWalking || isWalkingAway)
        {
            HandleMovement();
        }
        /*
        if (!isAttacking)
        {
            HandleMovement();
        }*/
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
            timerIdle = 5f;
            isIdling = true;

            isWalking = false;
            animator.SetBool("isWalking", isWalking);

            Debug.Log("arrived at crop");
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
                    // TODO: handle if companion is already befriended, dismantle it, for now give bits?

                    // add to companions
                    PlayerManager.Instance.PlayerFarmerData.AddCompanion(tempSOBefriend);
                    PlayerManager.Instance.SaveFarmerData();

                    // walk away
                    SetTargetOutsideScreen();
                    isWalkingAway = true;

                    Debug.Log("Companion befriended");
                }
                else
                {
                    // set the companion to walk away from the screen
                    SetTargetOutsideScreen();
                    isWalkingAway = true;

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

    private void CheckFlipOnEnemy(Vector2 dir)
    {
        // check sprite flip
        if (dir.x > 0 && faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
        else if (dir.x > 0f && !faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (dir.x < 0 && faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (dir.x < 0 && !faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
    }

    private void GenerateNewTarget()
    {
        currentTarget = UnityEngine.Random.Range(InitializerManager.Instance.GetScreenOffsetBound(), InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound());
        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(currentTarget, 0)).x;
    }

    private void SetTargetOutsideScreen()
    {
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

    /*
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
    }*/

    public void SetupBefriend(CompanionSO so, Vector2 cropPos)
    {
        tempSOBefriend = so;

        // set going to crop
        isGoingToCrop = true;

        // set crop target
        currentTarget = cropPos.x;

        Debug.Log("Setup befriend");
    }

    public void SetupRandomWalk()
    {
        // set going to crop
        isRandomWalking = true;

        GenerateNewTarget();
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


    public void SetAttacking(bool isAttacking, Vector2 playerDir)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        this.isAttacking = isAttacking;

        CheckFlipOnEnemy(playerDir);

        if (isAttacking)
        {
            timerAttack = 0;
        }

        animator.SetBool("IsAttacking", isAttacking);
    }
}
