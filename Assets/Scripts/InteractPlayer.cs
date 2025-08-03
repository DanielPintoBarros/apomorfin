using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPlayer : MonoBehaviour
{
    [SerializeField] private DialogCtrl dialogCtrl;
    private DialogLauncher dialogLauncher;

    [SerializeField] private float interactionRadius;
    public bool isInteracting;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (dialogCtrl.IsDialogActive())
            {
                dialogCtrl.NextPhrase();
            }
            else
            {
                isInteracting = true;
                TryStartDialog();
            }
        }
        else
        {
            isInteracting = false;
        }

    }

    private void TryStartDialog()
    {
        DialogLauncher nearestLauncher = FindNearestDialogLauncher();
        if (nearestLauncher != null)
        {
            nearestLauncher.Initialize();
        }
    }

    private DialogLauncher FindNearestDialogLauncher()
    {
        DialogLauncher[] launchers = FindObjectsOfType<DialogLauncher>();
        DialogLauncher nearest = null;
        float minDist = Mathf.Infinity;

        foreach (DialogLauncher launcher in launchers)
        {
            float dist = Vector3.Distance(transform.position, launcher.transform.position);
            if (dist < minDist && dist <= interactionRadius)
            {
                minDist = dist;
                nearest = launcher;
            }
        }

        return nearest;
    }
}
