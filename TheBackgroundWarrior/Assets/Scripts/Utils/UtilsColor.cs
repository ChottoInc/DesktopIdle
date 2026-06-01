using UnityEngine;

public static class UtilsColor
{
    public static Color CommonRarity = new Color(255f / 255f, 195f / 255f, 95f / 255f, 1f);
    public static Color UncommonRarity = new Color(96f / 255f, 180f / 255f, 255f / 255f, 1f);
    public static Color RareRarity = new Color(255f / 255f, 125f / 255f, 95f / 255f, 1f);

    public static Color GetColorByRarity(UtilsItem.CardRarity rarity)
    {
        switch (rarity)
        {
            default:
            case UtilsItem.CardRarity.Common: return CommonRarity;
            case UtilsItem.CardRarity.Uncommon: return UncommonRarity;
            case UtilsItem.CardRarity.Rare: return RareRarity;
        }
    }
}
