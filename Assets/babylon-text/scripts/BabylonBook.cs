using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class BabylonBook : MonoBehaviour 
{
	public SystemLanguage defaultLanguage = SystemLanguage.English;
	[TooltipAttribute("Path to the dictionary pool (bookshelf, heh). Relative to the Resources folder. Looks up for languagename.json files in the specified path (based on Unity's SystemLanguage enum). You can add a file prefix here if your files have one.")]
	public string pathToBookshelf = "languages/";
	SystemLanguage selectedLanguage;
	Dictionary<string,string> myDictionary = new Dictionary<string,string>();

	void Awake()
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
		//if language preferences has been saved
		if (PlayerPrefs.HasKey("SystemLanguage"))
		{
			return (SystemLanguage) System.Enum.Parse(typeof(SystemLanguage), PlayerPrefs.GetString("SystemLanguage"));
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
		PlayerPrefs.SetString("SystemLanguage",p_selectedLanguage.ToString());
		selectedLanguage = p_selectedLanguage;
	}

	/// <summary>
	/// simple method to access the dictionary
	/// </summary>
	/// <param name="key">Must be a valid key in the current dictionary</param>
	/// <returns>the value for the selected key</returns>
	public string GetString(string key)
	{
		if (myDictionary.ContainsKey(key))
		{
			return myDictionary[key];
		}
		return "<MISSING>";
	}
}
