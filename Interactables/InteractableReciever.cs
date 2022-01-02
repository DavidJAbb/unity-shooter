using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherit this base class for objects that take interaction signals - e.g. 'Light' can recieve signals from 'LightSwitch'
public class InteractableReciever : MonoBehaviour
{
    [HideInInspector]
    public bool canRecieve;

    private bool _isOn;
    public bool IsOn
    {
        get { return _isOn; }
        set { _isOn = value; }
    }

    public virtual void RecieveSignal(bool playerInTrigger)
    {
        // Override in the Reciever class e.g. public override void RecieveSignal() { // code here... }
        // playerInTrigger is only for trigger based stuff...
    }
}
