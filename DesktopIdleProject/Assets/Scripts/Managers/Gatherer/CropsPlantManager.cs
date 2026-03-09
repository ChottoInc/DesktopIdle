using UnityEngine;

public class CropsPlantManager : MonoBehaviour
{
    [SerializeField] WorldCropSlot slot1;
    [SerializeField] WorldCropSlot slot2;
    [SerializeField] WorldCropSlot slot3;
    [SerializeField] WorldCropSlot slot4;

    public Transform[] Crop1Transforms => slot1.CropTransforms;
    public Transform[] Crop2Transforms => slot2.CropTransforms;
    public Transform[] Crop3Transforms => slot3.CropTransforms;
    public Transform[] Crop4Transforms => slot4.CropTransforms;

    [Space(10)]
    [SerializeField] PlayerFarmer player;


    private CropData cropSlot1;
    private CropData cropSlot2;
    private CropData cropSlot3;
    private CropData cropSlot4;

    private float timerCropGrowth = 1f;


    public static CropsPlantManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (PlayerManager.Instance.PlayerFarmerData.Slot1CropData != null)
            SetCrop(0, PlayerManager.Instance.PlayerFarmerData.Slot1CropData, false);

        if (PlayerManager.Instance.PlayerFarmerData.Slot2CropData != null)
            SetCrop(1, PlayerManager.Instance.PlayerFarmerData.Slot2CropData, false);

        if (PlayerManager.Instance.PlayerFarmerData.Slot3CropData != null)
            SetCrop(2, PlayerManager.Instance.PlayerFarmerData.Slot3CropData, false);

        if (PlayerManager.Instance.PlayerFarmerData.Slot4CropData != null)
            SetCrop(3, PlayerManager.Instance.PlayerFarmerData.Slot4CropData, false);
    }

    private void Update()
    {
        if(timerCropGrowth <= 0)
        {
            UpdateCropData(0, cropSlot1);
            UpdateCropData(1, cropSlot2);
            UpdateCropData(2, cropSlot3);
            UpdateCropData(3, cropSlot4);

            UpdateWorldCrop(slot1, cropSlot1);
            UpdateWorldCrop(slot2, cropSlot2);
            UpdateWorldCrop(slot3, cropSlot3);
            UpdateWorldCrop(slot4, cropSlot4);

            timerCropGrowth = 1f;
        }
        else
        {
            timerCropGrowth -= Time.deltaTime;
        }
    }

    public void SetCrop(int slot, CropData data, bool animation)
    {
        WorldCropSlot selectedSlot = null;

        switch (slot)
        {
            case 0:
                cropSlot1 = data;
                selectedSlot = slot1;
                break;

            case 1:
                cropSlot2 = data;
                selectedSlot = slot2;
                break;

            case 2:
                cropSlot3 = data;
                selectedSlot = slot3;
                break;

            case 3:
                cropSlot4 = data;
                selectedSlot = slot4;
                break;
        }

        if (!animation)
        {
            SetCropSprite(slot, data);
        }
        else
        {
            CropSlotData cropSlotData = new CropSlotData(data, slot);

            player.SetSow(cropSlotData, selectedSlot.CropTransforms);
        }
    }

    public void SetCropSprite(int slot, CropData data)
    {
        Sprite currentSprite = data.GetCurrentSprite();
        WorldCropSlot selectedSlot = null;

        switch (slot)
        {
            case 0:
                cropSlot1 = data;
                selectedSlot = slot1;
                break;

            case 1:
                cropSlot2 = data;
                selectedSlot = slot2;
                break;

            case 2:
                cropSlot3 = data;
                selectedSlot = slot3;
                break;

            case 3:
                cropSlot4 = data;
                selectedSlot = slot4;
                break;
        }

        if (selectedSlot != null && currentSprite != null)
        {
            selectedSlot.SetSprite(currentSprite);
        }
    }

    private void UpdateCropData(int slot, CropData data)
    {
        if (data == null) return;

        data.AddGrowth(1);

        //Debug.Log("slot " + slot + ", crop growth: " + data.CurrentGrowth);
        PlayerManager.Instance.PlayerFarmerData.UpdateCropToSlot(data, slot);
    }

    private void UpdateWorldCrop(WorldCropSlot slot, CropData data)
    {
        if (data == null) return;

        Sprite currentSprite = data.GetCurrentSprite();
        if (slot != null && currentSprite != null)
            slot.SetSprite(currentSprite);
    }
}
