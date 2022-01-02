using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DestructibleProp : MonoBehaviour
{
    public GameObject destructibleGameObject;
    public float health = 100;
    public bool IsDestroyed { get; set; }


    private void Awake()
    {
        // Set up damage events from the parts
        DamageablePart[] parts = GetComponentsInChildren<DamageablePart>();
        foreach (DamageablePart part in parts)
        {
            part.OnDamage += Damage;
        }
    }


    private void Damage(Hit hit)
    {
        health -= hit.damage;
        Debug.Log($"{gameObject.name} took {hit.damage} pts of damage");

        if(health <= 0)
        {
            // TODO Spawn particles / debris etc.
            destructibleGameObject.SetActive(false); // Hide the prop visuals
        }
    }
}
