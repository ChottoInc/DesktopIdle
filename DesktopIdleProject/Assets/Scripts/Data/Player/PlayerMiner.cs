using System;
using UnityEngine;

public class PlayerMiner : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldownIdle = 1.5f;


    [Header("Combat")]
    [SerializeField] Transform checkRockPoint;
    [SerializeField] LayerMask rockLayer;


    private PlayerMinerData playerData;

    // ------ MOVEMENT VARS

    private Vector3 startScale;

    private float currentTarget;

    private bool isIdling;
    private float timerIdle;

    private bool dirLeft;

    private Rigidbody2D rb;

    // ------ ATTACK VARS

    private bool isRockDetected;

    private bool isSmashing;
    private float CooldownSmash => 1f / playerData.CurrentSmashSpeed;
    private float timerSmash;



    public event Action OnPerformSmash;




    public PlayerMinerData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= SaveFightData;
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
        if (isSmashing)
        {
            CheckSmash();
        }
    }

    private void FixedUpdate()
    {
        if (!isSmashing)
        {
            HandleMovement();

            CheckForRock();
        }
    }

    private void CheckSmash()
    {
        if (timerSmash <= 0)
        {
            OnPerformSmash?.Invoke();
            timerSmash = CooldownSmash;
        }
        else
        {
            timerSmash -= Time.deltaTime;
        }
    }

    private void CheckForRock()
    {
        if (isRockDetected) return;

        if (CheckRockAtPoint(checkRockPoint.position, 0.3f, rockLayer, out Collider2D hit))
        {
            Rock rock = hit.GetComponent<Rock>();

            if (!rock.IsSmashed && rock.RockIndex != RockSpawnManager.Instance.CurrentRockIndex - 1)
            {
                isRockDetected = true;
                SmashManager.Instance.StartSmash(rock);
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
        else if (vx < -0.01f && !faceRight)
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



    public void Setup(PlayerMinerData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += SaveFightData;
        }
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetSmashing(bool isSmashing)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        this.isSmashing = isSmashing;

        //reset detection rocks for update
        if (!isSmashing)
        {
            isRockDetected = false;
        }

        animator.SetBool("isSmashing", isSmashing);
    }

    public bool CheckRockAtPoint(Vector2 point, float radius, LayerMask rockMask, out Collider2D hitRock)
    {
        hitRock = Physics2D.OverlapCircle(point, radius, rockMask);
        return hitRock != null;
    }

    #region SAVE

    public void SaveFightData()
    {
        PlayerManager.Instance.UpdateMinerData(playerData);
        PlayerManager.Instance.SaveMinerData();
    }


    #endregion
}
