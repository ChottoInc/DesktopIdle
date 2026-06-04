using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Miner/Weapon To Sprite Data", fileName = "WeaponToSpriteData_")]
public class WeaponToSpriteSO : ListableGameDataSO
{
    [SerializeField] Sprite sprite;

    public Sprite Sprite => sprite;
}
