using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// "com.unity.nuget.newtonsoft-json": "3.0.2",

public class JsonDataService : IDataService
{
    private const string ENC_KEY = "lOU4bNn7TJyCLHpiW6PFu7Hj55mG39F7Eb5bWujFWcs=";
    private const string ENC_IV = "qObiuxvu1UWiS+QrjC8aJQ==";

    public bool SaveData<T>(string relativePath, T data, bool encrypted)
    {
        string path = Application.persistentDataPath + "/" + relativePath;

        try
        {
            // convert the data to json
            string json = JsonConvert.SerializeObject(data);

            SaveAtomic(path, json, encrypted);

            return true;
        }
        catch(Exception e)
        {
            Debug.LogError($"Unable to save due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public bool SaveAtomic(string path, string json, bool encrypted)
    {
        // get filename singularly from saving path
        string filenameWithoutExt = Path.GetFileNameWithoutExtension(path);
        string filenameExt = Path.GetExtension(path);
        string filename = filenameWithoutExt + filenameExt;

        // Example: data / temps / player_something.json.tmp
        string tempPath = Application.persistentDataPath + "/" + UtilsSave.GetTempsFolder() + "/" + filename + ".tmp";

        // Example: data / backups / player_something.json.bak
        string backupPath = Application.persistentDataPath + "/" + UtilsSave.GetBackupFolder() + "/" + filename + ".bak";

        // write to a temp file first
        if (!encrypted)
        {
            File.WriteAllText(tempPath, json);
        }
        else
        {
            // writes encrypted directly on temp data
            WriteEncryptedData(tempPath, json);
        }

        // if an old save exists, back it up
        if (File.Exists(path))
        {
            File.Copy(path, backupPath, true);
        }

        // only now replace the real file with the temp one, also copies encrypted
        File.Copy(tempPath, path, true);
        File.Delete(tempPath);


        return true;
    }

    private void WriteEncryptedData(string path, string json)
    {
        using FileStream stream = File.Create(path);

        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(ENC_KEY);
        aesProvider.IV = Convert.FromBase64String(ENC_IV);

        //Debug.Log("Start key: " + Convert.ToBase64String(aesProvider.Key));
        //Debug.Log("Start iv: " + Convert.ToBase64String(aesProvider.IV));

        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(
            stream,
            cryptoTransform,
            CryptoStreamMode.Write
        );

        cryptoStream.Write(Encoding.ASCII.GetBytes(json));
    }



    public T LoadData<T>(string relativePath, bool encrypted)
    {
        string path = Application.persistentDataPath + "/" + relativePath;

        if (!File.Exists(path))
        {
            // check if has backup
            // get filename singularly from saving path
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(path);
            string filenameExt = Path.GetExtension(path);
            string filename = filenameWithoutExt + filenameExt;

            // Example: data / backups / player_something.json.bak
            string backupPath = Application.persistentDataPath + "/" + UtilsSave.GetBackupFolder() + "/" + filename + ".bak";

            if (!File.Exists(backupPath))
            {
                //Debug.LogError($"Cannot load file at {path}");
                throw new FileNotFoundException($"{path} does not exists");
            }
            else
            {
                // has back file, copy file into path
                File.Copy(backupPath, path, true);
                Debug.LogWarning($"Recovered file from backups: {path}");
            }
        }

        try
        {
            T data;

            if (encrypted)
            {
                data = ReadEncryptedData<T>(path);
            }
            else
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }

            return data;
        }
        catch(Exception e)
        {
            //Debug.LogError($"Failed to load due to: {e.Message} {e.StackTrace}");
            throw new ConversionException($"Failed to load due to: {e.Message} {e.StackTrace}");
        }
    }

    private T ReadEncryptedData<T>(string path)
    {
        byte[] fileBytes = File.ReadAllBytes(path);

        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(ENC_KEY);
        aesProvider.IV = Convert.FromBase64String(ENC_IV);

        //Debug.Log("Start key: " + Convert.ToBase64String(aesProvider.Key));
        //Debug.Log("Start iv: " + Convert.ToBase64String(aesProvider.IV));

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
            aesProvider.Key,
            aesProvider.IV
        );

        using MemoryStream decryptionStream = new MemoryStream(fileBytes);

        using CryptoStream cryptoStream = new CryptoStream(
           decryptionStream,
           cryptoTransform,
           CryptoStreamMode.Read
       );

        using StreamReader reader = new StreamReader(cryptoStream);

        string result = reader.ReadToEnd();

        //Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or IV): {result}");
        return JsonConvert.DeserializeObject<T>(result);
    }



    

    
}