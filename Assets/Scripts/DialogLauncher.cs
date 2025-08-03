using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogLauncher : MonoBehaviour
{
    [SerializeField] DialogCtrl dialogCtrl;
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog conditionalDialog;

    [SerializeField] private Player player;
    [SerializeField] private bool mustCheckAbilities;
    [SerializeField] private string abilityName;

    public void Initialize()
    {
        if (dialogCtrl == null)
        {
            return;
        }

        if (mustCheckAbilities)
        {
            if (player.CheckCanLearnAbility(abilityName))
            {
                dialogCtrl.Initialize(conditionalDialog);
            }
            else
            {
                dialogCtrl.Initialize(dialog);
            }
        }
        else
        {
            dialogCtrl.Initialize(dialog);
        }
    }
}
