using UnityEngine;

[CreateAssetMenu(menuName = "Data/Shop/Bait Data", fileName = "BaitData_")]
public class ShopBaitSO : ShopItemSO
{
    [Space(10)]
    [SerializeField] BaitSO bait;
    [SerializeField] int quantity;

    public BaitSO BaitSO => bait;
    public int Quantity => quantity;
}
