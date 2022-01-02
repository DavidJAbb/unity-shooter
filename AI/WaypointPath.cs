using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public bool loopPath;
    public List<Waypoint> waypoints = new List<Waypoint>(); // User adds waypoints in the inspector


    private void Awake()
    {
        // Assign / correct the waypoint indices
        for(int i = 0; i < waypoints.Count; i++)
        {
            waypoints[i].index = i;
        }
    }


    // Get the closest waypoint - used for characters returning to paths
    // Remember to set the waypointIndex to the 
    public Waypoint GetClosestWaypoint(Vector3 position)
    {
        Waypoint _closestPoint = waypoints[0]; // Default is first point
        float closestDistanceSqr = Mathf.Infinity;

        for(int i = 0; i < waypoints.Count; i++)
        {
            Vector3 directionToTarget = waypoints[i].transform.position - position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                _closestPoint = waypoints[i];
            }
        }

        return _closestPoint;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawIcon(transform.position, "waypoint");

        Gizmos.color = Color.yellow;
        if (waypoints.Count > 0)
        {
            for(int i = 0; i < waypoints.Count; i++)
            {
                if (i == waypoints.Count-1)
                {
                    Gizmos.DrawLine(waypoints[i].transform.position, waypoints[0].transform.position);
                    var dir = waypoints[0].transform.position - waypoints[i].transform.position;
                    DrawArrow.ForGizmo(waypoints[i].transform.position, dir, 0.5f);
                }
                else
                {
                    Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i+1].transform.position);
                    var dir = waypoints[i+1].transform.position - waypoints[i].transform.position;
                    DrawArrow.ForGizmo(waypoints[i].transform.position, dir, 0.5f);
                }
            }
        }
    }
}
