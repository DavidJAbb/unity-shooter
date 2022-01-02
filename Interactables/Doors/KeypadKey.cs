using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadKey : MonoBehaviour, IInteractable
{
    public string keyCode;
    private KeypadLock _keypad;


    public void Init(KeypadLock keypad)
    {
        _keypad = keypad;
    }

    public void Interact()
    {
        _keypad.TryCode(keyCode);
    }
}
