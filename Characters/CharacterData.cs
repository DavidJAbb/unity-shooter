using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Data/Character Data")]
public class CharacterData : ScriptableObject
{
    public enum Faction
    {
        PLAYER,
        SOLDIER,
        MONSTER
    }
    [Header("Character Info")]
    [Tooltip("Used to determine if friendly or enemy")]
    public Faction faction;
    [Tooltip("Higher level threats have higher priority")]
    [Range(0, 10)]
    public int priority; // 0 to 10 - 10 is highest priority

    [Header("Health")]
    public float startingHealth = 100;
    public float maxHealth = 100;
    [Tooltip("Used for special characters")]
    public bool isInvulnerable;

    [Header("Sight Settings")]
    [Range(0, 360)]
    public float viewAngle = 80f; // For looking vs rotating to look at
    public float lookAtSpeed = 10f;
    public float lookYOffset = 1f;

    [Header("Movement Settings")]
    public float walkSpeed = 1.3f;
    public float runSpeed = 3.5f;
    public float turnSpeed = 5;

    [Header("Pathing Settings")]
    public float pathEndThreshold = 0.05f;
}