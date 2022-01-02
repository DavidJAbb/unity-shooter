using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : LockBase
{
    [Tooltip("The ID of the key")]
    public int keyID;

    public override void TryUnlock()
    {
        if (GameManager.Instance.player.inventory.HasKey(keyID))
        {
            door.Unlock();
        }
        else
        {
            Debug.Log("No matching key...");
        }
    }
}
