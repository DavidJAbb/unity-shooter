using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    public InteractableReciever reciever;
    private bool _inTrigger;
    public bool InTrigger
    {
        get { return _inTrigger;  }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            _inTrigger = true;

            if(reciever != null && reciever.canRecieve)
            {
                reciever.RecieveSignal(true);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _inTrigger = false;

            if (reciever != null && reciever.canRecieve)
            {
                reciever.RecieveSignal(false);
            }
        }
    }
}
