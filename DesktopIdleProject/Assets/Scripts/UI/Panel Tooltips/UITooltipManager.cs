using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITooltipManager : MonoBehaviour
{
    public const int ID_SHOW_NAME = 0;



    [SerializeField] UITooltipName tooltipName;


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

    public void Show(int idTooltip, string text, Vector2 position, bool fade = false)
    {
        switch(idTooltip)
        {
            default:
            case ID_SHOW_NAME: tooltipName.Show(text, position, fade); break;
        }
    }

    public void Hide(int idTooltip,  bool fade = false)
    {
        switch (idTooltip)
        {
            default:
            case ID_SHOW_NAME: tooltipName.Hide(fade); break;
        }
    }
}
