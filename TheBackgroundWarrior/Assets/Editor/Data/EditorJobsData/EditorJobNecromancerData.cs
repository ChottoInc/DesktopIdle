using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerJobNecromancerSO))]
public class EditorJobNecromancerData : Editor
{
    private const int ID_APTITUDE = 0;
    private const int ID_SUMMON = 1;
    private const int ID_MIGHT = 2;
    private const int ID_LIFESPAN = 3;
    private const int ID_HORDE = 4;
    private const int ID_LUCK = 5;

    private const int ID_BASE_EXP = 7;
    private const int ID_EXPO_EXP = 8;
    private const int ID_FLAT_EXP = 9;



    private const int GAIN_PER_LEVEL = 1;
    private const int MAX_LEVEL = 2;

    private const int COL_LEVEL_EXP = 1;

    PlayerJobNecromancerSO m_Script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Script = (PlayerJobNecromancerSO)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Load Data From SO"))
        {
            ReadData();
        }
    }

    private void ReadData()
    {
#if UNITY_EDITOR

        CultureInfo en_us = CultureInfo.GetCultureInfo("en-US");

        // get filepath from so
        List<string> datas = UtilsGeneral.GetFileStrings(m_Script.DataPath);

        // first read warrior statistics
        for (int i = 0; i < 6; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float gain = float.Parse(parts[GAIN_PER_LEVEL], en_us);
            int maxLevel = int.Parse(parts[MAX_LEVEL], en_us);

            switch (i)
            {
                case ID_APTITUDE:
                    m_Script.SetPerLevelGainAptitude(gain);
                    m_Script.SetMaxLevelAptitude(maxLevel);
                    break;

                case ID_SUMMON:
                    m_Script.SetPerLevelGainSummon(gain);
                    m_Script.SetMaxLevelSummon(maxLevel);
                    break;

                case ID_MIGHT:
                    m_Script.SetPerLevelGainMight(gain);
                    m_Script.SetMaxLevelMight(maxLevel);
                    break;

                case ID_LIFESPAN:
                    m_Script.SetPerLevelGainLifespan(gain);
                    m_Script.SetMaxLevelLifespan(maxLevel);
                    break;

                case ID_HORDE:
                    m_Script.SetPerLevelGainHorde(gain);
                    m_Script.SetMaxLevelHorde(maxLevel);
                    break;

                case ID_LUCK:
                    m_Script.SetPerLevelGainLuck(gain);
                    m_Script.SetMaxLevelLuck(maxLevel);
                    break;
            }
        }

        for (int i = 7; i < 10; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float value = float.Parse(parts[COL_LEVEL_EXP], en_us);

            switch (i)
            {
                case ID_BASE_EXP: m_Script.SetBaseExpGrowth(value); break;
                case ID_EXPO_EXP: m_Script.SetExpoExpGrowth(value); break;
                case ID_FLAT_EXP: m_Script.SetFlatExpGrowth(value); break;
            }
        }

        EditorUtility.SetDirty(m_Script);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

#endif
    }
}
