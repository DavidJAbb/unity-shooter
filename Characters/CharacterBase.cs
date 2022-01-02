using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Body and Head Setup")]
    [Tooltip("Used for location / weapon aiming")]
    public Transform body;
    [Tooltip("Used for animation rigging aim / look")]
    public Transform head;

    [Header("Health")]
    public float startingHealth = 100;
    public float maxHealth = 100;

    public float Health { get; set; }
    public bool IsDead { get; set; }


    public virtual void Init(float startingHealth)
    {
        Health = startingHealth;

        // Max health check
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }

        // Set up damage events from the body parts
        DamageablePart[] parts = GetComponentsInChildren<DamageablePart>();
        foreach(DamageablePart part in parts)
        {
            part.OnDamage += Damage;
        }
    }


    public virtual void Damage(Hit hit)
    {
        Health -= hit.damage;
        Debug.Log($"{gameObject.name} took {hit.damage} pts of damage");
    }


    public virtual void Heal(float healAmount)
    {
        Health += healAmount;

        // Max health check
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }
    }
}
