using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BabylonText : MonoBehaviour
{
    public char keySeparator = '@';
    char textSeparator = ' ';

    Text myText;

    void Awake()
    {
        myText = GetComponent<Text>();
    }

    void Start()
    {
        //get keys from Text's content
        string[] keys = myText.text.Split(keySeparator);
        //nullify the Text's content
        myText.text = "";
        //compose the final Text's content with the values of that keys
        foreach (string key in keys)
        {
            myText.text += BabylonCore.instance.GetString(key) + textSeparator;
        }
    }
}
