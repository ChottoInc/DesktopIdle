using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            UpdateFiles(true);
        }
        
        if (GUILayout.Button("Show file"))
        {
            var dict = GetLocalizedDictionary("file");
            foreach (var item in dict)
            {
                Debug.Log("" + item.Key + ": " + item.Value);
            }
        }
    }

    private void UpdateFiles(bool fromFile)
    {
#if UNITY_EDITOR

        // init with default dictionaries
        UtilsTextLimits.Initialize();
        UtilsText.Initialize();

        // make copies here
        Dictionary<string, string> defAllText = new Dictionary<string, string>(UtilsText.GeneralDictionary);
        Dictionary<string, string> defItemNames = new Dictionary<string, string>(UtilsText.ItemNamesTextDictionary);
        Dictionary<string, string> defItemDescs = new Dictionary<string, string>(UtilsText.ItemDescsTextDictionary);
        Dictionary<string, string> defCredits = new Dictionary<string, string>(UtilsText.CreditsTextDictionary);
        Dictionary<string, string> defHelp = new Dictionary<string, string>(UtilsText.HelpTextDictionary);

        // refill with file values
        UtilsText.FillValuesWithLang(UtilsGeneral.Language.Eng);
        string logDir = Path.Combine(Application.persistentDataPath, "Data");
        logDir = Path.Combine(logDir, "Localise");
        Directory.CreateDirectory(logDir);

        string allTextString = "General";
        string itemNamesString = "ItemNames";
        string itemDescsString = "ItemDescs";
        string creditsString = "Credits";
        string helpString = "Help";
        string finalString = "_eng.json";

        // keys present in default and not from file
        var missingPairsGeneralText = defAllText.Where(pair => !UtilsText.GeneralDictionary.ContainsKey(pair.Key));
        /*foreach (var pair in missingPairsAllText)
        {
            Debug.Log("key: " + pair.Key + ", val: " + pair.Value);
        }*/
        UtilsText.GeneralDictionary = MergeWithNonPresent(UtilsText.GeneralDictionary, missingPairsGeneralText);

        var missingPairsItemNames = defItemNames.Where(pair => !UtilsText.ItemNamesTextDictionary.ContainsKey(pair.Key));
        UtilsText.ItemNamesTextDictionary = MergeWithNonPresent(UtilsText.ItemNamesTextDictionary, missingPairsItemNames);

        var missingPairsItemDescs = defItemDescs.Where(pair => !UtilsText.ItemDescsTextDictionary.ContainsKey(pair.Key));
        UtilsText.ItemDescsTextDictionary = MergeWithNonPresent(UtilsText.ItemDescsTextDictionary, missingPairsItemDescs);

        var missingPairsCredits = defCredits.Where(pair => !UtilsText.CreditsTextDictionary.ContainsKey(pair.Key));
        UtilsText.CreditsTextDictionary = MergeWithNonPresent(UtilsText.CreditsTextDictionary, missingPairsCredits);

        var missingPairsHelp = defHelp.Where(pair => !UtilsText.HelpTextDictionary.ContainsKey(pair.Key));
        UtilsText.HelpTextDictionary = MergeWithNonPresent(UtilsText.HelpTextDictionary, missingPairsHelp);

        /*
        WriteOnFile(logDir, GetFileName(allTextString, finalString), UtilsText.GeneralDictionary);
        WriteOnFile(logDir, GetFileName(itemNamesString, finalString), UtilsText.ItemNamesTextDictionary);
        WriteOnFile(logDir, GetFileName(itemDescsString, finalString), UtilsText.ItemDescsTextDictionary);
        WriteOnFile(logDir, GetFileName(creditsString, finalString), UtilsText.CreditsTextDictionary);
        WriteOnFile(logDir, GetFileName(helpString, finalString), UtilsText.HelpTextDictionary);*/

        var jsonGeneral = Newtonsoft.Json.JsonConvert.SerializeObject(UtilsText.GeneralDictionary);
        var jsonItemNames = Newtonsoft.Json.JsonConvert.SerializeObject(UtilsText.ItemNamesTextDictionary);
        var jsonItemDescs = Newtonsoft.Json.JsonConvert.SerializeObject(UtilsText.ItemDescsTextDictionary);
        var jsonCredits = Newtonsoft.Json.JsonConvert.SerializeObject(UtilsText.CreditsTextDictionary);
        var jsonHelp = Newtonsoft.Json.JsonConvert.SerializeObject(UtilsText.HelpTextDictionary);

        File.WriteAllText(Path.Combine(logDir, GetFileName(allTextString, finalString)), jsonGeneral);
        File.WriteAllText(Path.Combine(logDir, GetFileName(itemNamesString, finalString)), jsonItemNames);
        File.WriteAllText(Path.Combine(logDir, GetFileName(itemDescsString, finalString)), jsonItemDescs);
        File.WriteAllText(Path.Combine(logDir, GetFileName(creditsString, finalString)), jsonCredits);
        File.WriteAllText(Path.Combine(logDir, GetFileName(helpString, finalString)), jsonHelp);

        //return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile.text);

#endif
    }

    private static Dictionary<string, string> GetLocalizedDictionary(string filepath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(Path.Combine("files/localize/eng", "Help" + "_eng"));
        if (jsonFile == null)
        {
            Debug.LogWarning($"Localization file not found: {filepath}");
            return new Dictionary<string, string>();
        }
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile.text);
    }

    private Dictionary<string, string> MergeWithNonPresent(Dictionary<string, string> dict, IEnumerable<KeyValuePair<string, string>> pairs)
    {
        foreach (var pair in pairs)
        {
            dict.Add(pair.Key, pair.Value);
        }
        return dict;
    }

    private string GetFileName(string first, string final)
    {
        return first + final;
    }

    private void WriteOnFile(string path, string filename, Dictionary<string, string> dict)
    {
        string COL1 = "ID";
        string COL2 = "ENG";
        string COL3 = "LIMITS";

        string logFilePath = Path.Combine(path, filename);
        StreamWriter logWriter = new StreamWriter(logFilePath, false);
        logWriter.AutoFlush = true;

        logWriter.WriteLine($"{COL1},{COL2},{COL3}");

        int counterLines = 0;
        foreach (var pair in dict)
        {
            if(counterLines != dict.Count - 1)
                logWriter.WriteLine(string.Format("{0}, \"{1}\",{2},", pair.Key,pair.Value, UtilsTextLimits.GetCharLimit(pair.Key)));
            else
                logWriter.Write(string.Format("{0}, \"{1}\",{2}", pair.Key, pair.Value, UtilsTextLimits.GetCharLimit(pair.Key)));

            counterLines++;
        }

        logWriter.Close();
        logWriter = null;
    }
}
