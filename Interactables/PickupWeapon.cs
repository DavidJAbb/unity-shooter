using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PickupWeapon : MonoBehaviour, IInteractable
{
    public WeaponType weaponType;
    public int weaponIndex;

    public void Interact()
    {
        // Add weapon to inventory
        GameManager.Instance.player.inventory.AddWeapon(weaponType, weaponIndex);

        // Remove pickup from world
        Destroy(gameObject, 0.05f);
    }
}
