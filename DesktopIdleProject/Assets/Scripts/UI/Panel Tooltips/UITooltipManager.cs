using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITooltipManager : MonoBehaviour
{
    public const int ID_SHOW_NAME = 0;
    public const int ID_SHOW_CARD = 1;



    [SerializeField] UITooltipName tooltipName;
    [SerializeField] UITooltipCard tooltipCard;


    public static UITooltipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show(TooltipManagerData tooltipData, Vector2 position, bool fade = false)
    {
        switch(tooltipData.idTooltip)
        {
            default:
            case ID_SHOW_NAME: tooltipName.Show(tooltipData.textName, position, fade); break;
            case ID_SHOW_CARD: tooltipCard.Show(tooltipData.cardSO, fade); break;
        }
    }

    public void Hide(int idTooltip,  bool fade = false)
    {
        switch (idTooltip)
        {
            default:
            case ID_SHOW_NAME: tooltipName.Hide(fade); break;
            case ID_SHOW_CARD: tooltipCard.Hide(fade); break;
        }
    }
}

public struct TooltipManagerData
{
    public int idTooltip;

    // tooltip name
    public string textName;

    // tooltip card
    public CardSO cardSO;
}