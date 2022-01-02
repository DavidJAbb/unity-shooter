using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Readable : MonoBehaviour, IInteractable
{
    public string readableName;
    [TextArea] public string readableContent;


    public void Interact()
    {
        // Show readable on UI...
        GameManager.Instance.readableManager.LoadReadable(this);
    }
}
