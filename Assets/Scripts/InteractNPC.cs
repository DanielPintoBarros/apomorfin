using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractNPC : MonoBehaviour
{
    [SerializeField] InteractPlayer interactPlayer;
    [SerializeField] DialogCtrl dialogCtrl;

    [SerializeField] private UnityEvent pressedBtn;

    [SerializeField] GameObject fBtn;
    [SerializeField] GameObject dialogBox;

    private bool canExecute;

    // Update is called once per frame
    void Update()
    {
        if (canExecute)
        {
            if (interactPlayer.isInteracting == true)
            {
                pressedBtn.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        { 
            canExecute = true;
            fBtn.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canExecute = false;
            fBtn.SetActive(false);
            dialogBox.gameObject.SetActive(false);
            dialogCtrl.currentDialog = null;
            dialogCtrl.count = 0;
        }
    }
}
