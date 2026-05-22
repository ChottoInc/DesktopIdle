using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomUtilsManager))]
public class EditorCustomUtilsManager : Editor
{
    CustomUtilsManager m_Script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_Script = (CustomUtilsManager)target;

        if (GUILayout.Button("Update english text files"))
        {
            UpdateFile();
        }
    }

    private void UpdateFile()
    {
#if UNITY_EDITOR

        UtilsText.Initialize();
        
        string logDir = Path.Combine(Application.persistentDataPath, "Data");
        logDir = Path.Combine(logDir, "Localise");
        Directory.CreateDirectory(logDir);

        string allTextString = "AllText";
        string itemNamesString = "ItemNames";
        string itemDescsString = "ItemDescs";
        string creditsString = "Credits";
        string helpString = "Help";
        string finalString = "_eng.csv";

        WriteOnFile(logDir, GetFileName(allTextString, finalString), UtilsText.AllTextDictionary);
        WriteOnFile(logDir, GetFileName(itemNamesString, finalString), UtilsText.ItemNamesTextDictionary);
        WriteOnFile(logDir, GetFileName(itemDescsString, finalString), UtilsText.ItemDescsTextDictionary);
        WriteOnFile(logDir, GetFileName(creditsString, finalString), UtilsText.CreditsTextDictionary);
        WriteOnFile(logDir, GetFileName(helpString, finalString), UtilsText.HelpTextDictionary);

#endif
    }

    private string GetFileName(string first, string final)
    {
        return first + final;
    }

    private void WriteOnFile(string path, string filename, Dictionary<string, string> dict)
    {
        string COL1 = "ID";
        string COL2 = "ENG";

        string logFilePath = Path.Combine(path, filename);
        StreamWriter logWriter = new StreamWriter(logFilePath, false);
        logWriter.AutoFlush = true;

        logWriter.WriteLine($"{COL1},{COL2},");

        int counterLines = 0;
        foreach (var pair in dict)
        {
            if(counterLines != dict.Count - 1)
                logWriter.WriteLine(string.Format("{0}, \"{1}\",", pair.Key,pair.Value));
            else
                logWriter.Write(string.Format("{0}, \"{1}\"", pair.Key, pair.Value));

            counterLines++;
        }

        logWriter.Close();
        logWriter = null;
    }
}
