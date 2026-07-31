using TMPro;
using UnityEngine;

public class UITooltipMapInfo : UITooltipBase
{
    [Header("Texts")]
    [SerializeField] TMP_Text _textMonsters;
    [SerializeField] TMP_Text _textCards;

    public override void Appear(TooltipManagerData data, bool fade, Vector2 position)
    {
        if (!SettingsManager.Instance.AreTooltipsOn) return;

        _textMonsters.text = data.possibleEnemies;
        _textCards.text = data.possibleCards;

        base.Appear(data, fade, position);
    }
}
