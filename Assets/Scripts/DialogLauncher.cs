using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogLauncher : MonoBehaviour
{
    [SerializeField] DialogCtrl dialogCtrl;
    [SerializeField] Dialog dialog;

    public void Initialize()
    {
        if (dialogCtrl == null)
        {
            return;
        }

        dialogCtrl.Initialize(dialog);
    }
}
