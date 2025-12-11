using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Card Data", fileName = "CardData_")]
public class CardSO : ItemSO
{
    [Space(10)]
    [SerializeField] UtilsItem.CardRarity cardRarity;

    [TextArea]
    [SerializeField] string cardDescription;


    public UtilsItem.CardRarity CardRarity => cardRarity;

    public string CardDescription => cardDescription;
}
