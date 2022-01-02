using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearSightVolume : MonoBehaviour
{
    public CharacterSenses senses;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(Tags.Player))
        {
            // Player entered trigger...
            senses.targetInNearSightVolume = true;
        }
    }


    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag(Tags.Player))
        {
            // Player left trigger...
            senses.targetInNearSightVolume = false;
        }
    }
}
