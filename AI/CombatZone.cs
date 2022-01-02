using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    public List<CoverObject> coverObjects = new List<CoverObject>();
    public List<Vector3> fleePositions = new List<Vector3>();
    public bool IsActive { get; set; }

    // Activate if player is in the zone
    private void OnTriggerEnter(Collider other)
    {
        if (IsActive)
            return;

        if(other.gameObject.CompareTag(Tags.Player))
        {
            IsActive = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsActive)
            return;

        if (other.gameObject.CompareTag(Tags.Player))
        {
            IsActive = true;
        }
    }

    // Deactivate if player is NOT in the zone
    private void OnTriggerExit(Collider other)
    {
        if (!IsActive)
            return;

        if (other.gameObject.CompareTag(Tags.Player))
        {
            IsActive = false;
        }
    }

    // Returns the first cover position facing the player from the cover object that is nearest to npc AND in front of target
    public CoverPosition BestCoverPositionFacingTarget(Transform npcTransform, Transform targetTransform)
    {
        // Sort position list by distance to target
        coverObjects.Sort(delegate(CoverObject a, CoverObject b)
        {
            float squaredRangeA = (a.transform.position - npcTransform.position).sqrMagnitude;
            float squaredRangeB = (b.transform.position - npcTransform.position).sqrMagnitude;
            return squaredRangeA.CompareTo(squaredRangeB);
        });

        // Is the cover object in front of the player?
        for(int i = 0; i < coverObjects.Count; i++)
        {
            // If Cover Object is in front of player
            Vector3 toTarget1 = (coverObjects[i].transform.position - targetTransform.position).normalized;
            if (Vector3.Dot(toTarget1, targetTransform.forward) > 0.5f)
            {
                Debug.Log($"Cover object {coverObjects[i].gameObject.name} is in front!");
                for (int j = 0; j < coverObjects[i].coverPositions.Length; j++)
                {
                    // If the cover position is roughly facing the target AND it's not already reserved...
                    if(Vector3.Dot(coverObjects[i].coverPositions[j].transform.forward, targetTransform.forward) < 0 && !coverObjects[i].coverPositions[j].isReserved)
                    {
                        Debug.Log($"Cover pos {coverObjects[i].coverPositions[j].gameObject.name} is facing the target! And isn't reserved!");
                        return coverObjects[i].coverPositions[j];
                    }
                }
            }
        }

        // TODO improvements?
        // Exclude cover objects that are too close to the player - only over X range?
        // Sort cover positions by distance to player and start with closest? Good for bigger objects?

        return null;
    }

    // Get the nearest flee position that's behind the character's position
    public Vector3 NearestFleePosition(Vector3 npcPosition, Vector3 npcDirection)
    {
        // Sort list by distance to NPC
        fleePositions.Sort(delegate (Vector3 a, Vector3 b)
        {
            float squaredRangeA = (a - npcPosition).sqrMagnitude;
            float squaredRangeB = (b - npcPosition).sqrMagnitude;
            return squaredRangeA.CompareTo(squaredRangeB);
        });

        for(int i = 0; i < fleePositions.Count; i++)
        {
            Vector3 toTarget = (fleePositions[i] - npcPosition).normalized;
            // TODO this should actually be comparing the direction from the target to the enemy, I think, so runs away from player...
            if (Vector3.Dot(toTarget, npcDirection) < 0.5f)
            {
                return fleePositions[i];
            }
        }

        Debug.Log("no suitable flee position available...");
        return Vector3.zero;
    }
}
