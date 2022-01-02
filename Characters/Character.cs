using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : CharacterBase
{
    public enum Faction
    {
        PLAYER,
        SOLDIER,
        MONSTER
    }
    [Header("Character Info")]
    public string characterName;
    [Tooltip("Used to determine if friendly or enemy")]
    public Faction faction;
    [Tooltip("Higher level threats have higher priority")]
    [Range(0, 10)]
    public int priority; // 0 to 10 - 10 is highest priority

    [Tooltip("Used for special characters")]
    public bool isInvulnerable;

    [Header("Sight Settings")]
    [Range(0, 360)]
    public float viewAngle = 80f; // For looking vs rotating to look at
    public float lookAtSpeed = 10f;
    public float lookYOffset = 1f;

    [Header("Aiming Settings")]
    public float aimSmoothTime = 0.3f; // TODO Move this to data?
    private Vector3 aimVelocity = Vector3.zero;

    [Header("Movement Settings")]
    public float walkSpeed = 1.3f;
    public float runSpeed = 3.5f;
    public float turnSpeed = 5;

    [Header("Pathing Settings")]
    public float pathEndThreshold = 0.05f;

    [Header("Gun")]
    public CharacterGun gun;
    public GameObject weaponDropPrefab;
    public Transform weaponDropTransform;

    [Header("Patrol Route (Optional)")]
    public WaypointPath patrolPath;
    public int WaypointIndex { get; set; }

    [Header("Combat Zone")]
    public CombatZone combatZone;

    [Header("Debug")]
    public TMP_Text textMesh;
    private static string newLine = "\n";
    private string debugCanvasText;

    // Private components
    private Collider _collider; // The simple character collider
    public CharacterBehaviour Behaviour { get; set; }
    public NavMeshAgent Agent { get; set; }
    public CharacterAnimation Animation { get; set; }
    public CharacterMovement Movement { get; set; }
    public CharacterSenses Senses { get; set; }

    // AI variables
    public Vector3 MovePosition { get; set; } // Used by move tasks
    public Vector3 TurnTarget { get; set; } // Turn tasks always turn to this position
    public int BulletsInClip { get; set; }

    public Vector3 Sighting { get; set; }
    public Distraction Distraction { get; set; }

    public CharacterBase CurrentTarget { get; set; } // Player target 
    public bool CurrentTargetLost { get; set; } // Set to true when lost sight - set to false when investigation / search concluded
    public Vector3 TargetLastKnownPosition { get; set; } // Used for investigation / search
    public bool TargetPreviouslySeen { get; set; } // Enemy has seen player once - used to prevent going back to chill idle
    public bool UnderFire { get; set; }
    public bool InCombat { get; set; }
    public bool InCover { get; set; }

    // public bool HasPlayerTarget { get; set; }
    // public Transform Target { get; set; }
    // public Vector3 LastKnownPosition { get; set; }
    // public float TimeSinceTargetLastSeen { get; set; }


    void Awake()
    {
        _collider = GetComponent<Collider>();

        Agent = GetComponent<NavMeshAgent>();
        Behaviour = GetComponentInChildren<CharacterBehaviour>();
        Movement = GetComponent<CharacterMovement>();
        Animation = GetComponent<CharacterAnimation>();
        Senses = GetComponentInChildren<CharacterSenses>();
    }


    void Start()
    {
        // Init Health
        Init(startingHealth);

        // Init components
        Movement.Init(this);
        Senses.Init(this);
        Behaviour.Init(this);

        // Init weapons
        if(gun != null)
        {
            gun.Init();
            BulletsInClip = 20;
        }
    }


    private void Update()
    {
        if (IsDead)
            return;

        // Sense checks
        Senses.SightChecks();

        // Behaviour checks
        if(Behaviour.CheckDecisionNextFrame)
        {
            Behaviour.CheckDecision();
        }
        Behaviour.UpdateBehaviours();

        // Aim at the player's head if we have a player target and we are aiming
        if(CurrentTarget != null && Animation.IsAiming)
        {
            AR_AimAtTarget(GameManager.Instance.player.head.position);
        }

        // Debugging
        Debug.DrawRay(Senses.sightTransform.position, Senses.sightTransform.forward, Color.magenta);
        Debug.DrawRay(transform.position, transform.forward, Color.white);
    }


    // Animation Rigging functions
    public void AR_LookAtTarget(Vector3 target)
    {
        // Get the adjusted look position...
        Vector3 direction = target - Senses.sightTransform.position;
        Ray lookRay = new Ray(Senses.sightTransform.position, direction);
        Vector3 lookPos = lookRay.GetPoint(5);
        // Set the position of the aim target
        Animation.lookTarget.position = lookPos;
        // Check weight is set
        if (Animation.lookRig.weight == 0)
        {
            Animation.lookRig.weight = 1;
        }
    }


    public void AR_AimAtTarget(Vector3 target)
    {
        // Check weight is set
        if (Animation.aimRig.weight != 1)
        {
            Animation.aimRig.weight = 1;
        }

        // Get the adjusted look position...
        Vector3 direction = target - Senses.sightTransform.position;
        float angle = Vector3.Angle(transform.forward, direction); // Check angle between capsule and target - not head. This is deliberate!
        bool isInFront = angle < 80;

        if(isInFront)
        {
            Ray lookRay = new Ray(Senses.sightTransform.position, direction);
            Vector3 lookPos = lookRay.GetPoint(10);
            // Set the position of the aim target
            Animation.aimTarget.position = Vector3.SmoothDamp(Animation.aimTarget.position, lookPos, ref aimVelocity, aimSmoothTime);
        }
        else
        {
            AR_ReturnToDefaultAimPos();
        }
    }


    public void AR_ReturnToDefaultAimPos()
    {
        Animation.aimTarget.localPosition = Vector3.SmoothDamp(Animation.aimTarget.localPosition, Animation.aimDefaultPos, ref aimVelocity, aimSmoothTime);
        // Animation.aimRig.weight = 0;
    }


    public void DisableSight()
    {
        Senses.enabled = false;
    }


    public void DisableCharacterCollision()
    {
        _collider.enabled = false;
    }


    public override void Damage(Hit hit)
    {
        base.Damage(hit);
        UnderFire = true;
        Behaviour.CheckDecisionNextFrame = true; // Damage is a factor in decision making
        Debug_UpdateStatusText();
    }


    public void DisableWeapon()
    {
        // Disable weapon in hand
        if (gun != null)
        {
            gun.gameObject.SetActive(false);
        }
    }


    public void SpawnLoot()
    {
        // Spawn loot drops
        GameObject weaponClone = Instantiate(weaponDropPrefab, weaponDropTransform.position, Quaternion.identity); // Spawn the object
        // TODO spawn other drops like ammo / components?
    }


    // SHARED AI FUNCTIONS
    
    // Patrol / Waypoint
    public int GetClosestWaypointIndex()
    {
        int index = patrolPath.GetClosestWaypoint(transform.position).index;
        return index;
    }

    // DISTRACTIONS
    public void SetDistraction(Distraction distraction)
    {
        // Only add if we don't already have a distraction - or if it has the same or higher priority
        if (Distraction == null || Distraction != null && distraction.priority >= Distraction.priority)
        {
            // New audio distraction...
            Distraction = distraction;
            Behaviour.CheckDecisionNextFrame = true;
        }
    }

    public void ClearDistraction()
    {
        Distraction = null;
        Behaviour.CheckDecisionNextFrame = true;
    }

    // TARGETS
    public void SetTarget(CharacterBase target)
    {
        CurrentTarget = target;
        TargetPreviouslySeen = true; // So enemies don't go back to chill state...
        Behaviour.CheckDecisionNextFrame = true;
    }

    public void TargetLost(Vector3 lastKnownPos)
    {
        CurrentTargetLost = true;
        TargetLastKnownPosition = lastKnownPos;
        Behaviour.CheckDecisionNextFrame = true;
    }

    // SIGHTINGS
    public void SetSighting(Vector3 sightingPos)
    {
        Sighting = sightingPos;
        Behaviour.CheckDecisionNextFrame = true;
    }

    // Debug
    void OnDrawGizmos()
    {
        if(Distraction != null)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Distraction.transform.position, 0.5f);
        }
    }

    public virtual void Debug_UpdateStatusText()
    {
        if (textMesh == null)
            return;

        debugCanvasText = "";
        debugCanvasText += characterName + newLine;
        debugCanvasText += $"Faction: {faction}" + newLine;
        debugCanvasText += $"Health: {Health}" + newLine;
        debugCanvasText += $"Bullets: {BulletsInClip}" + newLine;
        debugCanvasText += newLine;

        debugCanvasText += $"-{Behaviour.CurrentState}" + newLine;
        if (Behaviour.CurrentState.CurrentTask != null)
        {
            debugCanvasText += $"--{ Behaviour.CurrentState.CurrentTask}" + newLine;
        }

        textMesh.text = debugCanvasText;
    }
}
