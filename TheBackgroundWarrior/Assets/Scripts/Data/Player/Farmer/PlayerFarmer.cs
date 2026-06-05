using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFarmer : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;

    [Space(10)]
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldownIdle = 3f;

    [Space(10)]
    [SerializeField] AnimationClip sowClip;
    [SerializeField] AnimationClip mowClip;


    private float timer5Mins;

    private PlayerFarmerData playerData;


    // --------- MOVEMENT VARS

    private bool canInitialMove;

    private bool isWalking;

    private enum FarmerAction { Sowing, Mowing }

    private bool isSowing;
    private bool canSow;

    private bool isMowing;
    private bool canMow;


    private Vector3 startScale;

    private float currentTarget;
    private Vector2 currentDirection;

    private bool isIdling;
    private float timerIdle;

    private Rigidbody2D rb;

    // ------ FARMER VARS

    private Queue<Transform> farmPlotsToMow;
    private Queue<CropSlotData> farmPlotsToMowData;
    private int currentFarmPlotToMow;

    private Queue<Transform> farmPlotsToSow;
    private Queue<CropSlotData> farmPlotsToSowData;
    private int currentFarmPlotToSow;

    public event Action<int, int> OnStatChange;




    public PlayerFarmerData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeFarmer;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // every 5 mins give some exp to the player
        if (timer5Mins <= 0)
        {
            playerData.AddExp(UtilsFarmer.PASSIVE_EXP);
            timer5Mins = UtilsGeneral.TIMER_5MIN_IN_SECONDS;

            PlayerManager.Instance.UpdateFarmerData(playerData);
            PlayerManager.Instance.SaveFarmerData();
        }
        else
        {
            timer5Mins -= Time.deltaTime;
        }
    }

    public void Setup(PlayerFarmerData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeFarmer;
        }
    }

    private void Start()
    {
        startScale = spriteRenderer.transform.localScale;

        StartCoroutine(CoSpawned());

        timer5Mins = UtilsGeneral.TIMER_5MIN_IN_SECONDS;
    }


    private void FixedUpdate()
    {
        if (!isSowing && !isMowing)
        {
            HandleMovement();
        }
        else
        {
            if(canSow)
                GoToFarmPlot(FarmerAction.Sowing);
            else if(canMow)
                GoToFarmPlot(FarmerAction.Mowing);
        }
    }

    private IEnumerator CoSpawned()
    {
        yield return new WaitForSeconds(2f);

        // change rb type
        rb.bodyType = RigidbodyType2D.Kinematic;

        // enable movement
        canInitialMove = true;

        GenerateNewTarget();

        isWalking = true;
        animator.SetBool("isWalking", isWalking);
    }

    /// <summary>
    /// Handles walking, if distant from target walk, else idle
    /// </summary>
    private void HandleMovement()
    {
        if (!canInitialMove) return;

        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f && !isIdling)
        {
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("isWalking", isWalking);
            }

            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += speed * Time.fixedDeltaTime * (Vector3)currentDirection;

            CheckFlip();
        }
        else
        {
            // handles idling timer before move again
            if (!isIdling)
            {
                isWalking = false;
                animator.SetBool("isWalking", isWalking);

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

    /// <summary>
    /// Handles movement to first farm plot when sowing, starts a coroutine for next ones
    /// </summary>
    private void GoToFarmPlot(FarmerAction fAction)
    {
        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 1f && !isIdling)
        {
            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += speed * Time.fixedDeltaTime * (Vector3)currentDirection;

            CheckFlip();
        }
        else
        {
            if(fAction == FarmerAction.Sowing)
            {
                // reset to stop the movement
                canSow = false;

                isWalking = false;
                animator.SetBool("isWalking", isWalking);

                // wait for sow animation and call next farm plot
                StartCoroutine(CoWaitSowAnimation());
            }
            else if(fAction == FarmerAction.Mowing)
            {
                // reset to stop the movement
                canMow = false;

                isWalking = false;
                animator.SetBool("isWalking", isWalking);

                // wait for sow animation and call next farm plot
                StartCoroutine(CoWaitMowAnimation());
            }
        }
    }

    private void CheckFlip()
    {
        // check sprite flip
        float vx = currentDirection.x;
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

    public void AddSow(CropSlotData cropSlotData, Transform[] farmPlots)
    {
        // reset index
        //currentFarmPlot = 0;

        farmPlotsToSow ??= new Queue<Transform>();
        farmPlotsToSowData ??= new Queue<CropSlotData>();

        foreach (var farmPlot in farmPlots)
        {
            farmPlotsToSow.Enqueue(farmPlot);
        }

        farmPlotsToSowData.Enqueue(cropSlotData);

        // handles if it's not already sowing, or else just add to queues
        if (!isSowing)
        {
            // set sowing so a new target isn't generated
            isSowing = true;

            // start sow plots
            NextPlot(FarmerAction.Sowing,farmPlotsToSow.Dequeue());
        }
    }

    public void AddMow(CropSlotData cropSlotData, Transform[] farmPlots, bool needResetCrop)
    {
        // reset index
        //currentFarmPlot = 0;

        farmPlotsToMow ??= new Queue<Transform>();
        farmPlotsToMowData ??= new Queue<CropSlotData>();

        foreach (var farmPlot in farmPlots)
        {
            farmPlotsToMow.Enqueue(farmPlot);
        }

        farmPlotsToMowData.Enqueue(cropSlotData);

        // handles if it's not already mowing, or else just add to queues
        if (!isMowing)
        {
            // set sowing so a new target isn't generated
            isMowing = true;

            // start sow plots
            NextPlot(FarmerAction.Mowing, farmPlotsToMow.Dequeue());
        }
    }

    private void NextPlot(FarmerAction fAction, Transform farmPlot)
    {
        // set next farm plot X
        currentTarget = farmPlot.position.x;

        // disable isIdling
        if (isIdling)
        {
            isIdling = false;
        }

        // set to walking before reaching farm plot
        if (!isWalking)
        {
            isWalking = true;
            animator.SetBool("isWalking", isWalking);
        }

        if(fAction == FarmerAction.Sowing)
        {
            // set already next farm plot index
            currentFarmPlotToSow++;

            // start move
            canSow = true;
        }
        else if(fAction == FarmerAction.Mowing)
        {
            // set already next farm plot index
            currentFarmPlotToMow++;

            // start move
            canMow = true;
        }
    }

    private IEnumerator CoWaitSowAnimation()
    {
        // tell animator to do the sowing animation
        animator.SetTrigger("Sow");

        yield return new WaitForSeconds(sowClip.length);

        // every 4 plots reset and dequeue datas to set sprite
        if (currentFarmPlotToSow == 4)
        {
            currentFarmPlotToSow = 0;
            CropSlotData slotData = farmPlotsToSowData.Dequeue();

            // set sprites
            CropsPlantManager.Instance.SetCropSprite(slotData.slot, slotData.cropData);
        }

        if (farmPlotsToSow.Count > 0)
        {
            NextPlot(FarmerAction.Sowing, farmPlotsToSow.Dequeue());
        }
        else
        {
            isSowing = false;

            // set new target
            GenerateNewTarget();
        }
    }

    private IEnumerator CoWaitMowAnimation()
    {
        // tell animator to do the sowing animation
        animator.SetTrigger("Mow");

        yield return new WaitForSeconds(mowClip.length);

        // every 4 plots reset and dequeue datas to set sprite
        if (currentFarmPlotToMow == 4)
        {
            currentFarmPlotToMow = 0;
            CropSlotData slotData = farmPlotsToMowData.Dequeue();

            // harvest
            Harvest(slotData);

            // reset crop
            ResetCrop(slotData);
        }

        if (farmPlotsToMow.Count > 0)
        {
            NextPlot(FarmerAction.Mowing, farmPlotsToMow.Dequeue());
        }
        else
        {
            isMowing = false;

            // set new target
            GenerateNewTarget();
        }
    }

    private void Harvest(CropSlotData slotData)
    {
        // TODO: might change with new stat? influenced by luck and greeenthumb??
        PlayerManager.Instance.Inventory.AddItem(slotData.cropData.CropSO.Id, 4);
        Debug.Log("Adding: " + slotData.cropData.CropSO.Id);
        PlayerManager.Instance.SaveInventoryData();
    }

    private void ResetCrop(CropSlotData slotData)
    {
        CropData currentCrop = null;

        switch (slotData.slot)
        {
            case 0: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot1CropData; break;
            case 1: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot2CropData; break;
            case 2: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot3CropData; break;
            case 3: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot4CropData; break;
        }

        if (currentCrop != null)
        {
            currentCrop.ResetGrowth();
            CropsPlantManager.Instance.SetCrop(slotData.slot, currentCrop, false);
        }
    }


    public void HandleSwitchScene()
    {

    }


    #region SAVE

    public void SaveFarmerData()
    {
        PlayerManager.Instance.UpdateFarmerData(playerData);
        PlayerManager.Instance.SaveFarmerData();
    }

    #endregion

    #region HANDLE EVENTS FROM FARMER DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveFarmerData();
    }

    private void OnStatChangeFarmer(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
