using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PickupKey : MonoBehaviour, IInteractable
{
    public string keyName;
    public int keyID;
    private Key _key;

    private void Start()
    {
        _key = new Key(keyName, keyID);
    }

    public void Interact()
    {
        // Add to inventory
        GameManager.Instance.player.inventory.AddKey(_key);

        // Remove from world
        Destroy(gameObject, 0.05f);
    }
}
