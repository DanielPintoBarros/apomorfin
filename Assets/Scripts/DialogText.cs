using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogText
{
    [SerializeField] [TextArea(1, 4)] private string phrase;
    

    public string GetPhrase()
    {
        return phrase;
    }
}

