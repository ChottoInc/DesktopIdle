using System.Collections.Generic;

using static UtilsText;


public static class UtilsTextLimits
{
    public static Dictionary<string, int> _dictLimits;

    public static void Initialize()
    {
        _dictLimits = new Dictionary<string, int>()
        {
            { text_settings_gameplay_autobattle, 16 },
        };
    }

    public static int GetCharLimit(string key)
    {
        return _dictLimits.TryGetValue(key, out int limit) ? limit : 0;
    }
}
