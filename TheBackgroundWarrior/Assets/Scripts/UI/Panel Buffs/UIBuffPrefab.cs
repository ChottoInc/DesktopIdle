using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffPrefab : MonoBehaviour
{
    [SerializeField] Image _imageBuff;
    [SerializeField] TMP_Text _textName;
    [SerializeField] TMP_Text _textDesc;

    [Space(10)]
    [SerializeField] TMP_Text _textTimer;


    private Buff _buff;
    private BuffSO _buffSO;


    public void Setup(Buff buff)
    {
        _buff = buff;
        _buffSO = UtilsBuffs.GetBuffSOByType(buff.BuffType);

        // set sprite from product of recipe
        _imageBuff.sprite = _buffSO.Sprite;

        _textName.text = _buffSO.ItemName;
        _textDesc.text = string.Format(_buffSO.ItemDesc, Mathf.RoundToInt(_buff.StartDuration/60f));

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        float totalSeconds = _buff.RemainingTime;

        int hours = (int)(totalSeconds / 3600);
        int minutes = (int)(totalSeconds % 3600 / 60);
        int seconds = (int)(totalSeconds % 60);

        if (hours > 0)
            _textTimer.text = $"{hours:00}:{minutes:00}:{seconds:00}";
        else
            _textTimer.text = $"{minutes:00}:{seconds:00}";
    }
}
