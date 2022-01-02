using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PickupHealth : MonoBehaviour, IInteractable
{
    public float healthAmount;

    public void Interact()
    {
        if(GameManager.Instance.player.Health < GameManager.Instance.player.maxHealth)
        {
            GameManager.Instance.player.Heal(healthAmount);

            Destroy(gameObject, 0.05f);
        }
    }
}
