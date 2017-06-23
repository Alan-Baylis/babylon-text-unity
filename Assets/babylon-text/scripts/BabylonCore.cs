using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class BabylonCore : ScriptableObject
{
    public bool forceDefault;
    public SystemLanguage defaultLanguage = SystemLanguage.English;
    [TooltipAttribute("Path to the dictionary pool (bookshelf, heh). Relative to the Resources folder. Looks up for languagename.json files in the specified path (based on Unity's SystemLanguage enum). You can add a file prefix here if your files have one.")]
    public string pathToBookshelf = "languages/";
    SystemLanguage selectedLanguage;
    Dictionary<string, string> myDictionary = new Dictionary<string, string>();

    string missingIdentifier = "<MISSING>";

    static BabylonCore _instance;
    public static BabylonCore instance
    {
        get
        {
            if (_instance == null)
            {
                BabylonCore[] foundItems = (BabylonCore[])Resources.FindObjectsOfTypeAll(typeof(BabylonCore));
                if (foundItems.Length > 0)
                {
                    _instance = foundItems[0];
                    _instance.Initialize();
                }
            }

            return _instance;
        }
    }

    void Initialize()
    {
        selectedLanguage = GetSystemLanguage();

        Debug.Log("Loading " + selectedLanguage.ToString() + " dictionary.");
        TextAsset targetFile = Resources.Load<TextAsset>(pathToBookshelf + selectedLanguage.ToString());

        if (targetFile == null)
        {
            Debug.LogWarning(selectedLanguage.ToString() + " language file was not found. Defaulting to " + defaultLanguage.ToString() + " instead.");
            targetFile = Resources.Load<TextAsset>(pathToBookshelf + defaultLanguage.ToString());
        }

        if (targetFile == null)
        {
            Debug.LogError("Default " + defaultLanguage.ToString() + " language file was also not found. Please add some language files to your project, and if they do exist already, please check Path To Bookshelf is set correctly in this script.");
            targetFile = Resources.Load<TextAsset>(pathToBookshelf + defaultLanguage.ToString());
            return;
        }

        string json = targetFile.ToString();

        myDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }

    SystemLanguage GetSystemLanguage()
    {
        if (forceDefault)
        {
            return defaultLanguage;
        }
        //if language preferences has been saved
        if (PlayerPrefs.HasKey("SystemLanguage"))
        {
            return (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), PlayerPrefs.GetString("SystemLanguage"));
        }
        //if not
        return Application.systemLanguage;
    }

    /// <summary>
    /// Saves the selected language in PlayerPrefs, and changes it in the script
    /// </summary>
    /// <param name="p_selectedLanguage">the language you want to set as new</param>
    public void SetSystemLanguage(SystemLanguage p_selectedLanguage)
    {
        PlayerPrefs.SetString("SystemLanguage", p_selectedLanguage.ToString());
        selectedLanguage = p_selectedLanguage;
    }

    /// <summary>
    /// Simple method to access the dictionary
    /// </summary>
    /// <param name="key">Must be a valid key in the current dictionary</param>
    /// <returns>the value for the selected key, or a missing string indicator</returns>
    public string GetString(string key)
    {
        if (myDictionary.ContainsKey(key))
        {
            return myDictionary[key];
        }
        return missingIdentifier;
    }

    /// <summary>
    /// Simple method to access the dictionary
    /// </summary>
    /// <param name="key">Must be a valid key in the current dictionary</param>
    /// <returns>the value for the selected key, or the same key if no translation was found</returns>
    public string Translate(string key)
    {
        string translation = GetString(key);
        return translation.Contains(missingIdentifier) ? key : translation;
    }
}
