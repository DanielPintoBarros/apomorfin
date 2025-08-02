using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogCtrl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI npcName, text, continueBtn;

    [SerializeField] GameObject dialogBox;

    private int count = 0;
    private Dialog currentDialog;

    internal void Initialize(Dialog dialog)
    {
        count = 0;
        currentDialog = dialog;

        NextPhrase();
    }

    public void NextPhrase()
    {
        if (currentDialog == null)
        {
            return;
        }

        if (count == currentDialog.GetPhrases().Length)
        {
            dialogBox.gameObject.SetActive(false);
            currentDialog = null;
            count = 0;
        }

        npcName.text = currentDialog.GetNPCName();
        text.text = currentDialog.GetPhrases()[count].GetPhrase();

        dialogBox.gameObject.SetActive(true);
        count++;
    }
}
