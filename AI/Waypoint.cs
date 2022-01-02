using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public enum WaypointType
    {
        DEFAULT,
        WAIT_FOR_TIME, // generic pause for x seconds
        WAIT_INDEFINITELY // used for playing a special animation, a conversation etc.
    }
    public WaypointType waypointType;

    [Range(0, 30)]
    public float waitTime;

    // Set whether the type events happen evertime, only once, or randomly
    public enum UseType
    {
        EVERY_TIME,
        ONE_SHOT,
        RANDOM
    }
    public UseType useType;


    [HideInInspector]
    public int index;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.15f);
    }
}
