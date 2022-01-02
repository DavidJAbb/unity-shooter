using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// For returning to paths after a distraction
public class TASK_RETURN_TO_PATROL_PATH : BehaviourTask
{
    public float waypointReturnRange = 20;

    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - TASK_RETURN_TO_PATROL_PATH");

        // Return if there are no waypoints set up
        if (character.patrolPath == null)
        {
            Debug.Log("TASK_RETURN_TO_PATROL_PATH can't complete because there's no path.");
            StopTask();
        }
        else
        {
            // If next waypoint is within range then use that - if not then get the closest waypoint
            if (Vector3.Distance(character.transform.position, character.patrolPath.waypoints[character.WaypointIndex].transform.position) > waypointReturnRange)
            {
                character.WaypointIndex = character.GetClosestWaypointIndex(); // Start at the closest waypoint and go from there... for returning to paths after a distraction
            }

            base.StartTask();
        }
    }

    public override void UpdateTask()
    {
        character.Movement.WalkToPosition(character.patrolPath.waypoints[character.WaypointIndex].transform.position);

        if (character.Movement.FinishedMove())
        {
            character.WaypointIndex++;
            if (character.WaypointIndex == character.patrolPath.waypoints.Count)
            {
                character.WaypointIndex = 0;
                character.Movement.StopMoving();
                StopTask();
            }
        }

        base.UpdateTask();
    }

    public override void StopTask()
    {
        character.Movement.StopMoving();
        base.StopTask();
    }
}
