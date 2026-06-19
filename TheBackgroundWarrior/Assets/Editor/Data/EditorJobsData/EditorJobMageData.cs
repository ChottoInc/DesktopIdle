using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerJobMageSO))]
public class EditorJobMageData : Editor
{
    private const int ID_INSIGHT = 0;
    private const int ID_CASTSPEED = 1;
    private const int ID_SCHOLAR = 2;
    private const int ID_PROFICIENCY = 3;

    private const int ID_BASE_EXP = 5;
    private const int ID_EXPO_EXP = 6;
    private const int ID_FLAT_EXP = 7;



    private const int GAIN_PER_LEVEL = 1;
    private const int MAX_LEVEL = 2;

    private const int COL_LEVEL_EXP = 1;

    PlayerJobMageSO m_Script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Script = (PlayerJobMageSO)target;

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
        for (int i = 0; i < 4; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float gain = float.Parse(parts[GAIN_PER_LEVEL], en_us);
            int maxLevel = int.Parse(parts[MAX_LEVEL], en_us);

            switch (i)
            {
                case ID_INSIGHT:
                    m_Script.SetPerLevelGainInsight(gain);
                    m_Script.SetMaxLevelInsight(maxLevel);
                    break;

                case ID_CASTSPEED:
                    m_Script.SetPerLevelGainCastSpeed(gain);
                    m_Script.SetMaxLevelCastSpeed(maxLevel);
                    break;

                case ID_SCHOLAR:
                    m_Script.SetPerLevelGainScholar(gain);
                    m_Script.SetMaxLevelScholar(maxLevel);
                    break;

                case ID_PROFICIENCY:
                    m_Script.SetPerLevelGainProficiency(gain);
                    m_Script.SetMaxLevelProficiency(maxLevel);
                    break;
            }
        }

        for (int i = 5; i < 8; i++)
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
