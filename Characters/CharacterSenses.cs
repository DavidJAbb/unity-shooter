using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSenses : MonoBehaviour
{
    public Transform sightTransform;
    public LayerMask sightMask;
    [HideInInspector]
    public bool targetInNearSightVolume;

    private Character _character;

    private float _halfConeSize;
    private bool _targetInRange;

    // Public Behaviours Vars
    public bool TargetInSight { get; set; }
    public bool LostSightOfTarget { get; set; }

    // Sight States
    // GLIMPSED - Player is seen for a moment - I saw something over there...
    // CONFIRMED - Player sighting is confirmed - Target aquired!
    // HIDING - Player breaks line of sight - He's here somewhere! He can't have gone far!
    // LOST - Player is lost - We lost him / He's gone / He got away

    private float _inSight;
    private float _outOfSight;
    private Vector3 lastKnownSighting;
    private CharacterBase playerTarget;


    public void Init(Character character)
    {
        _character = character;
        _halfConeSize = character.viewAngle / 2;
        _inSight = 0;
        _outOfSight = 0;

        playerTarget = GameManager.Instance.player;
    }


    private void OnTriggerEnter(Collider col)
    {
        // Player enters sphere collider
        if(col.CompareTag(Tags.Player))
        {
            _targetInRange = true;
        }

        // Distraction enters sphere collider
        if (col.CompareTag("SightDistraction"))
        {
            _character.SetDistraction(col.GetComponent<Distraction>());
        }
    }


    private void OnTriggerExit(Collider col)
    {
        // Player leaves sphere collider
        if (col.CompareTag(Tags.Player))
        {
            _targetInRange = false;
        }
    }


    public void SightChecks()
    {
        if(playerTarget)
        {
            // SHORT RANGE
            if (targetInNearSightVolume)
            {
                // Raycast to body to confirm sight
                if (!Physics.Linecast(sightTransform.position, playerTarget.body.position, sightMask))
                {
                    lastKnownSighting = playerTarget.body.position;

                    if (!TargetInSight)
                    {
                        // TODO Increment short reaction time timer?
                        TargetInSight = true;
                        _outOfSight = 0;
                        _character.SetTarget(playerTarget);
                        Debug.Log("Near Sight Confirmed!");
                    }
                }
                else
                {
                    if (TargetInSight)
                    {
                        _outOfSight += Time.deltaTime; // Out of sight
                    }
                }
            }
            // MID TO FAR RANGE
            else if (!targetInNearSightVolume && _targetInRange)
            {
                Vector3 targetVector = playerTarget.body.position - sightTransform.position;
                float angle = Vector3.Angle(sightTransform.forward, targetVector);
                bool isInFront = angle < _halfConeSize;

                // In front...
                if (isInFront)
                {
                    // In front and in line of sight
                    // TODO Raycast to head in combat / chest rest of the time to confirm sight
                    if (!Physics.Linecast(sightTransform.position, playerTarget.body.position, sightMask))
                    {
                        // Player is in viewcone
                        Debug.DrawLine(sightTransform.position, playerTarget.body.position, Color.green);

                        // Record latest sighting position - TODO also need movement vector?
                        lastKnownSighting = playerTarget.body.position;

                        if (!TargetInSight)
                        {
                            _inSight += Time.deltaTime; // In sight...
                            if (_inSight >= 1.5f)
                            {
                                TargetInSight = true;
                                _outOfSight = 0;
                                _character.SetTarget(playerTarget);
                                Debug.Log("Sight Confirmed!");
                            }
                        }
                    }
                    else
                    {
                        // Player is in range and in front - but no line of sight
                        Debug.DrawLine(sightTransform.position, playerTarget.body.position, Color.blue);

                        if (TargetInSight)
                        {
                            _outOfSight += Time.deltaTime; // Out of sight
                        }
                    }
                }
                else
                {
                    // Not in front...
                    Debug.DrawLine(sightTransform.position, playerTarget.body.position, Color.red);

                    if (TargetInSight)
                    {
                        _outOfSight += Time.deltaTime; // Out of sight
                    }
                }
            }

            // Lose sight if out of sight for X seconds...
            if (TargetInSight && _outOfSight >= 20f)
            {
                TargetInSight = false;
                _inSight = 0;
                _character.TargetLost(lastKnownSighting); // Trigger an investigation or search
                Debug.Log("Sight lost...");
            }
        }
    }


    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lastKnownSighting, 0.5f);
    }
}
