using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PickupAmmo : MonoBehaviour, IInteractable
{
    public AmmoType ammoType;
    public int ammoAmount;

    public void Interact()
    {
        // Check if player can pickup ammo
        if(GameManager.Instance.player.inventory.CanPickupAmmoOfType(ammoType, ammoAmount))
        {
            // Add to inventory
            GameManager.Instance.player.inventory.AddAmmo(ammoType, ammoAmount);

            // Remove from world
            Destroy(gameObject, 0.05f);
        }
    }
}
