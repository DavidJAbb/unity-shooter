using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageablePart : MonoBehaviour, IDamageable
{
    public float damageMultiplier = 1; // e.g. Head does double damage / arm does half.
    public Action<Hit> OnDamage;


    public void TakeDamage(float damageAmount)
    {
        // Do nothing...
    }


    public void TakeHit(Hit hit) {
        if(OnDamage != null)
        {
            hit.damage *= damageMultiplier;
            OnDamage(hit);
        }

        // DECALS
        /*
        RaycastHit rayHit;
        Ray ray = new Ray(hit.point, hit.direction);
        // Check if ray hits anything
        if (Physics.Raycast(ray, out rayHit, 6, GameManager.Instance.envLayers))
        {
            if (rayHit.collider != null)
            {
                // Paint decals
                Quaternion normalRotation = Quaternion.LookRotation(rayHit.normal);
                GameManager.Instance.surfaceManager.SpawnPrefab(GameManager.Instance.surfaceManager.bloodDecal_default, rayHit.point, normalRotation, rayHit.collider.transform, true);
            }
        }
        */
    }
}
