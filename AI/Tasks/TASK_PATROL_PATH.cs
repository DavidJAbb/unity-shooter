using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_PATROL_PATH : BehaviourTask
{
    private float _timer = 0f;

    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        character.Debug_UpdateStatusText();
        // Debug.Log("NEW TASK - TASK_PATROL_PATH");
        // Return if there are no waypoints set up
        if (character.patrolPath == null)
        {
            Debug.Log("TASK_PATROL_PATH can't complete because there's no path.");
            StopTask();
        }
        else
        {
            character.WaypointIndex = 0;
            base.StartTask();
        }
    }

    public override void UpdateTask()
    {
        character.Movement.WalkToPosition(character.patrolPath.waypoints[character.WaypointIndex].transform.position);

        if (character.Movement.FinishedMove())
        {
            if (character.patrolPath.waypoints[character.WaypointIndex].waypointType == Waypoint.WaypointType.DEFAULT)
            {
                character.WaypointIndex++;
            }
            else if (character.patrolPath.waypoints[character.WaypointIndex].waypointType == Waypoint.WaypointType.WAIT_FOR_TIME)
            {
                character.Movement.StopMoving();

                _timer += Time.deltaTime;
                if(_timer >= character.patrolPath.waypoints[character.WaypointIndex].waitTime)
                {
                    character.WaypointIndex++;
                }
            }
            else if (character.patrolPath.waypoints[character.WaypointIndex].waypointType == Waypoint.WaypointType.WAIT_INDEFINITELY)
            {
                // Stop the task and return when the other action is complete? Or keep it in the patrol task?
                // TODO...
                character.WaypointIndex++; // Just ignore for now...
            }

            // If we reach the end of the path...
            if (character.WaypointIndex == character.patrolPath.waypoints.Count)
            {
                // Reset the waypoint index and stop the task
                character.WaypointIndex = 0;
                // character.Movement.StopMoving();
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
