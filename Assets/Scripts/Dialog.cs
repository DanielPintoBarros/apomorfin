using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog 
{
    [SerializeField] private DialogText[] phrases;

    [SerializeField] private string npcName;

    public DialogText[] GetPhrases()
    {
        return phrases;
    }

    public string GetNPCName()
    {
        return npcName;
    }
}
