using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITooltipBuffPrefab : MonoBehaviour
{
    [SerializeField] Image _imageIcon;
    [SerializeField] TMP_Text _textTimer;

    private Buff _buff;

    public void Setup(Buff buff)
    {
        _buff = buff;

        _imageIcon.sprite = UtilsBuffs.GetBuffSpriteByType(buff.BuffType);

        UpdateTimerUI();
    }

    private void Update()
    {
        if (_buff == null) return;

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
