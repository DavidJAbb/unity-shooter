/* Player Weapon base class */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public WeaponData data; // Weapon attributes are held in this scriptable object

    public virtual void Init(Player player) { }

    public virtual void Equip() { }

    public virtual void Unequip() { }

    public virtual void OnPrimaryHold() { }

    public virtual void OnPrimaryRelease() { }

    public virtual void Reload() { }

    public virtual void UpdateHUD() { }
}
