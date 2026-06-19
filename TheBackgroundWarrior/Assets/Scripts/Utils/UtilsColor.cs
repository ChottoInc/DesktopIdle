using UnityEngine;

public static class UtilsColor
{
    public const string BACKGROUND_BAR_LEARN_MAGE = "FFE4BA";
    public const string FILL_BAR_LEARN_MAGE = "E29B2E";


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

    public static Color GetColorHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color)) 
        {
            return color;
        }
        return Color.white;
    }
}
