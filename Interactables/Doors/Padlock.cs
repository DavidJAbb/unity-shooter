using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock : LockBase, IDamageable
{
    public float health;


    public void TakeDamage(float damageAmount)
    {
        // Do nothing...
    }

    // Player shoots the lock...
    public void TakeHit(Hit hit)
    {
        health -= hit.damage;

        if(health <= 0)
        {
            // TODO Spawn padlock model
            door.Unlock();
            gameObject.SetActive(false);
        }
    }
}
