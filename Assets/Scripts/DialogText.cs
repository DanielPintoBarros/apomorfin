using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogText
{
    [SerializeField] private string phrase, continueBtn;

    public string GetFrase()
    {
        return phrase;
    }

    public string GetContinueBtn()
    {
        return continueBtn;
    }
}

