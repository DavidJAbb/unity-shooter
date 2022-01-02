using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorHandle : MonoBehaviour, IInteractable
{
    public bool isInner;
    public UnityEvent interactionEvent;

    public void Interact()
    {
        interactionEvent.Invoke();
    }
}
