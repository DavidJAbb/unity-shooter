using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _curHealth;
    public float CurrentHealth
    {
        get { return _curHealth; }
    }

    private float _maxHealth;
    private Character character;

    // Death Event
    public delegate void DieEvent();

    public Vector3 LastHitDirection { get; set; }
    public event DieEvent OnDie;


    public void Init(float maxHealth, float startingHealth)
    {
        _maxHealth = maxHealth;
        _curHealth = startingHealth;

        // Max health check
        if (_curHealth > _maxHealth)
        {
            _curHealth = _maxHealth;
        }

        // Set up damage events from the body parts
        DamageablePart[] parts = GetComponentsInChildren<DamageablePart>();
        foreach(DamageablePart part in parts)
        {
            part.OnDamage += Damage;
        }

        character = GetComponent<Character>();
    }


    public void Damage(Hit hit)
    {
        _curHealth -= hit.damage;

        Debug.Log($"{this.gameObject.name} took {hit.damage} pts of damage"); // For testing

        // Send death event
        if (_curHealth <= 0)
        {
            if(OnDie != null)
            {
                OnDie();
            }
        }
    }


    public void Heal(float healAmount)
    {
        _curHealth += healAmount;

        // Max health check
        if (_curHealth > _maxHealth)
        {
            _curHealth = _maxHealth;
        }
    }
}
