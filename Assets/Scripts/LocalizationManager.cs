using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText; // 被本地化的文本用字典存起来

    private bool isReady;

    private readonly string missingTextString = "Localized text not found";

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this; 
        } 
        else if (instance != this)  // 被实例化时发现场景中已经有一个实例化对象
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLocalizedText(string fileName) 
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath)) 
        {
            string dataAsJson = File.ReadAllText(filePath,System.Text.Encoding.UTF8);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else 
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key]; 
        }
        return result; 
    }

    public bool GetIsReady()
    {
        return isReady; 
    }
}
