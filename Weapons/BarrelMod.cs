/* Barrel Mod - for optional silencers etc. */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelMod : MonoBehaviour
{
    //---------------Model Setup----------------\\
    [Header("Prefab Setup")]
    [Tooltip("The position that the bullet will come from.")]
    public Transform barrelEnd;
    [Tooltip("The muzzle flash particle system.")]
    public ParticleSystem muzzleFlash;

    [Header("Audio")]
    public AudioClip fireSound;
}
