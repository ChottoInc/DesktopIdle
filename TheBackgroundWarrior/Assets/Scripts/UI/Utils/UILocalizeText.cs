using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UILocalizeText : MonoBehaviour
{
    [SerializeField] string textId;

    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        RefreshText();
    }

    private void OnDisable()
    {
        
    }

    private void RefreshText()
    {
        //text.text = 
    }
}
