using UnityEngine;

public class CropData
{
    private CropSO cropSO;


    private float finalGrowthTime;
    private float currentGrowth;

    private int plantedSlot;


    public CropSO CropSO => cropSO;

    public float GrowthTime => finalGrowthTime;
    public float CurrentGrowth => currentGrowth;
    public int PlantedSlot => plantedSlot;



    public CropData(CropSO cropSO, int plantedSlot)
    {
        this.cropSO = cropSO;

        finalGrowthTime = cropSO.BaseGrowthTime;
        currentGrowth = 0;

        this.plantedSlot = plantedSlot;
    }

    public CropData(CropSaveData saveData)
    {
        cropSO = UtilsGather.GetCropById(saveData.cropId);

        finalGrowthTime = cropSO.BaseGrowthTime;
        currentGrowth = saveData.currentGrowth;

        plantedSlot = saveData.plantedSlot;
    }

    public Sprite GetCurrentSprite()
    {
        int maxSprites = CropSO.SpriteCrop.Length;
        float percGrowth = currentGrowth / finalGrowthTime;
        int spriteIndex = Mathf.FloorToInt(percGrowth * maxSprites);
        spriteIndex = Mathf.Clamp(spriteIndex, 0, maxSprites);
        return CropSO.SpriteCrop[spriteIndex];
    }

    public void AddGrowth(float t)
    {
        currentGrowth += t;
    }
}
