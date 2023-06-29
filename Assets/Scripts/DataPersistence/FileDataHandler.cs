using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class FileDataHandler 
{

    private string dataDirPath;
    private string dataFileName;
    private bool useDataEncryption = false;
    private readonly string encryptionCodeWord = "dataSafety";


    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useDataEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        try
        {
            string dataToLoad;
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad= reader.ReadToEnd();
                }
            }
            if (useDataEncryption)
            {
                dataToLoad = EncryptDecrypt(dataToLoad);
            }
            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
        } 

        catch(Exception e)
        {
            Debug.LogError(e);
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);//what if dont use true? it wont be that human readable and more compact

            if (useDataEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }


            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }

        

    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

}
