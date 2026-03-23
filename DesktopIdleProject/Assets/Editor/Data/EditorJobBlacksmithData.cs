using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerJobBlacksmithSO))]
public class EditorJobBlacksmithData : Editor
{
    private const int ID_CRAFTSPEED = 0;
    private const int ID_EFFICIENCY = 1;
    private const int ID_LUCK = 2;

    private const int ID_BASE_EXP = 4;
    private const int ID_EXPO_EXP = 5;

    private const int ID_HELMET_MAXHP_LINEAR = 7;
    private const int ID_HELMET_MAXHP_QUADRATIC = 8;

    private const int ID_ARMOR_DEF_LINEAR = 9;
    private const int ID_ARMOR_DEF_QUADRATIC = 10;


    private const int ID_GLOVES_ATKSPD_LINEAR = 11;
    private const int ID_GLOVES_ATKSPD_QUADRATIC = 12;

    private const int ID_GLOVES_CRITDMG_LINEAR = 13;
    private const int ID_GLOVES_CRITDMG_QUADRATIC = 14;

    private const int ID_BOOTS_DEF_LINEAR = 15;
    private const int ID_BOOTS_DEF_QUADRATIC = 16;
                         
    private const int ID_BOOTS_CRITRATE_LINEAR = 17;
    private const int ID_BOOTS_CRITRATE_QUADRATIC = 18;



    private const int GAIN_PER_LEVEL = 1;
    private const int MAX_LEVEL = 2;

    private const int COL_LEVEL_EXP = 1;

    private const int COL_GEAR_GROWTH = 1;

    PlayerJobBlacksmithSO m_Script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Script = (PlayerJobBlacksmithSO)target;

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
        for (int i = 0; i < 3; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float gain = float.Parse(parts[GAIN_PER_LEVEL], en_us);
            int maxLevel = int.Parse(parts[MAX_LEVEL], en_us);

            switch (i)
            {
                case ID_CRAFTSPEED:
                    m_Script.SetPerLevelGainCraftSpeed(gain);
                    m_Script.SetMaxLevelCraftSpeed(maxLevel);
                    break;

                case ID_EFFICIENCY:
                    m_Script.SetPerLevelGainEfficiency(gain);
                    m_Script.SetMaxLevelEfficiency(maxLevel);
                    break;

                case ID_LUCK:
                    m_Script.SetPerLevelGainLuck(gain);
                    m_Script.SetMaxLevelLuck(maxLevel);
                    break;
            }
        }

        for (int i = 4; i < 6; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float value = float.Parse(parts[COL_LEVEL_EXP], en_us);

            switch (i)
            {
                case ID_BASE_EXP: m_Script.SetBaseExpGrowth(value); break;
                case ID_EXPO_EXP: m_Script.SetExpoExpGrowth(value); break;
            }
        }

        // Helmet
        for (int i = 7; i < 9; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float value = float.Parse(parts[COL_GEAR_GROWTH], en_us);

            switch (i)
            {
                case ID_HELMET_MAXHP_LINEAR: m_Script.SetHelmetMaxHpLinearGrowth(value); break;
                case ID_HELMET_MAXHP_QUADRATIC: m_Script.SetHelmetMaxHpQuadraticGrowth(value); break;
            }
        }

        // Armor
        for (int i = 9; i < 11; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float value = float.Parse(parts[COL_GEAR_GROWTH], en_us);

            switch (i)
            {
                case ID_ARMOR_DEF_LINEAR: m_Script.SetArmorDefLinearGrowth(value); break;
                case ID_ARMOR_DEF_QUADRATIC: m_Script.SetArmorDefQuadraticGrowth(value); break;
            }
        }

        // Gloves
        for (int i = 11; i < 15; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float value = float.Parse(parts[COL_GEAR_GROWTH], en_us);

            switch (i)
            {
                case ID_GLOVES_ATKSPD_LINEAR: m_Script.SetGlovesAtkSpdLinearGrowth(value); break;
                case ID_GLOVES_ATKSPD_QUADRATIC: m_Script.SetGlovesAtkSpdQuadraticGrowth(value); break;
                case ID_GLOVES_CRITDMG_LINEAR: m_Script.SetGlovesCritDmgLinearGrowth(value); break;
                case ID_GLOVES_CRITDMG_QUADRATIC: m_Script.SetGlovesCritDmgQuadraticGrowth(value); break;
            }
        }

        // Boots
        for (int i = 15; i < 19; i++)
        {
            string[] parts = datas[i].Split(",", System.StringSplitOptions.None);

            float value = float.Parse(parts[COL_GEAR_GROWTH], en_us);

            switch (i)
            {
                case ID_BOOTS_DEF_LINEAR: m_Script.SetBootsDefLinearGrowth(value); break;
                case ID_BOOTS_DEF_QUADRATIC: m_Script.SetBootsDefQuadraticGrowth(value); break;
                case ID_BOOTS_CRITRATE_LINEAR: m_Script.SetBootsCritRateLinearGrowth(value); break;
                case ID_BOOTS_CRITRATE_QUADRATIC: m_Script.SetBootsCritRateQuadraticGrowth(value); break;
            }
        }

        EditorUtility.SetDirty(m_Script);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

#endif
    }
}
