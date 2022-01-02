using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    public WeaponData data; // All weapon attributes are held in this scriptable object.

    public virtual void Init() { }

    public virtual void Attack() { }

    public virtual void Reload() { }
}
