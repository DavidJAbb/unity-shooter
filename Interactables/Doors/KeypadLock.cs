using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadLock : LockBase
{
    public string keypadCode;

    private KeypadKey[] _keys;
    private string _attempt;
    private bool _canInteract;


    private void Awake()
    {
        // Set up the keys
        _keys = GetComponentsInChildren<KeypadKey>();
        for (int i = 0; i < _keys.Length; i++)
        {
            _keys[i].Init(this);
        }

        _attempt = string.Empty;
        _canInteract = true;
    }

    public override void TryUnlock()
    {
        // Play a sound?
    }

    // TODO make this an IEnumerator and add delays, graphics, sounds etc.
    public void TryCode(string keycode) 
    {
        if (!_canInteract)
            return;

        _attempt += keycode;

        if(_attempt.Length == 4 && door.isLocked)
        {
            Debug.Log($"Trying this code: {_attempt}...");

            if (_attempt == keypadCode)
            {
                _canInteract = false; // Stop further interactions after success
                door.Unlock();
                Debug.Log("Success! Correct code.");
            }
            else if(_attempt != keypadCode)
            {
                ResetCode();
                Debug.Log("Fail! Wrong code...");
            }
        }
    }

    public void ResetCode()
    {
        _attempt = string.Empty;
    }
}
