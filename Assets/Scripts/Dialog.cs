using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Dialog 
{
    [SerializeField] private DialogText[] phrases;

    [SerializeField] private string npcName;

    [SerializeField] UnityEvent action;

    public DialogText[] GetPhrases()
    {
        return phrases;
    }

    public string GetNPCName()
    {
        return npcName;
    }

    public void DoAction()
    {
        if (action != null)
        {
            action.Invoke();
        }
    }
}
