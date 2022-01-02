using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableSwitch : MonoBehaviour, IInteractable
{
    public UnityEvent interactionEvent;
    public InteractableTrigger trigger;


    public void Interact()
    {
        if(trigger != null)
        {
            if(trigger.InTrigger)
            {
                interactionEvent.Invoke();
            }
        }
        else
        {
            interactionEvent.Invoke();
        }
        /*
        if(reciever.canRecieve)
        {
            if(trigger != null)
            {
                reciever.RecieveSignal(trigger.InTrigger);
            }
            else
            {
                reciever.RecieveSignal(false);
            }
        }
        */
    }
}
