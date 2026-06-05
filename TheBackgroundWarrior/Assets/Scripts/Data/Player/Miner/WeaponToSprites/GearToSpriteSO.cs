using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Miner/Gear To Sprite Data", fileName = "GearToSpriteData_")]
public class GearToSpriteSO : ListableGameDataSO
{
    [SerializeField] Sprite sprite;

    public Sprite Sprite => sprite;
}
