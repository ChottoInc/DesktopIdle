using UnityEngine;

public class CropData
{
    private CropSO cropSO;


    private float baseGrowthTime;
    private float currentGrowth;

    private int plantedSlot;


    public CropSO CropSO => cropSO;

    public float GrowthTime => 
        baseGrowthTime -
        (baseGrowthTime * PlayerManager.Instance.PlayerFarmerData.CurrentGreenthumb);
    public float CurrentGrowth => currentGrowth;
    public int PlantedSlot => plantedSlot;



    public CropData(CropSO cropSO, int plantedSlot)
    {
        this.cropSO = cropSO;

        baseGrowthTime = cropSO.BaseGrowthTime;
        currentGrowth = 0;

        this.plantedSlot = plantedSlot;
    }

    public CropData(CropSaveData saveData)
    {
        cropSO = UtilsGather.GetCropById(saveData.cropId);

        baseGrowthTime = cropSO.BaseGrowthTime;
        currentGrowth = saveData.currentGrowth;

        plantedSlot = saveData.plantedSlot;
    }

    public Sprite GetCurrentSprite()
    {
        int maxSprites = CropSO.SpriteCrop.Length;
        float percGrowth = currentGrowth / GrowthTime;

        // set to max - 1, so when the growth is not 100%, the right sprite will be shown
        int spriteIndex;

        if(percGrowth >= 1f)
        {
            spriteIndex = maxSprites - 1;
            return CropSO.SpriteCrop[spriteIndex];
        }
        else
        {
            spriteIndex = Mathf.FloorToInt(percGrowth * (maxSprites - 1));
            return CropSO.SpriteCrop[spriteIndex];
        }
    }

    public void AddGrowth(float t)
    {
        currentGrowth += t;
    }
}