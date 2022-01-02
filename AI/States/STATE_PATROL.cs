using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_PATROL : BehaviourState
{
    [Range(0,30)]
    public float initialDelayTime = 2f;

    TASK_WAIT waitTask;
    BehaviourTask moveTask;


    public override void Init(Character character)
    {
        // Set up TASKS
        waitTask = new TASK_WAIT();
        moveTask = new TASK_MOVE_TO_POSITION();
        waitTask.Init(character);
        moveTask.Init(character);

        base.Init(character);
    }

    public override bool ConditionsMet()
    {
        if(character.patrolPath != null)
        {
            return true;
        }
        return false;
    }

    public override void EnterState()
    {
        Debug.Log("Entered STATE_PATROL");

        character.MovePosition = character.patrolPath.waypoints[character.WaypointIndex].transform.position;
        SetTask(moveTask);

        character.Animation.SetStance(CharacterAnimation.Stance.IDLE);

        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // WAITING
        if(CurrentTask == waitTask && CurrentTask.executionState == ExecutionState.COMPLETED)
        {
            character.MovePosition = character.patrolPath.waypoints[character.WaypointIndex].transform.position;
            SetTask(moveTask);
        }

        // MOVING
        if (CurrentTask == moveTask && CurrentTask.executionState == ExecutionState.COMPLETED)
        {
            // END OF PATH...
            if (character.WaypointIndex == character.patrolPath.waypoints.Count-1)
            {
                character.WaypointIndex = 0;

                if (character.patrolPath.loopPath)
                {
                    character.MovePosition = character.patrolPath.waypoints[character.WaypointIndex].transform.position;
                    SetTask(moveTask);
                }
                else if (!character.patrolPath.loopPath)
                {
                    // At this point should we go to idle? Idle will need higher priority? Maybe Idle is a task rather than a state?
                    waitTask.waitIndefinitely = true;
                    SetTask(waitTask);
                }
            }
            else if(character.WaypointIndex < character.patrolPath.waypoints.Count - 1)
            {
                // WAYPOINTS
                // Go to next waypoint
                if (character.patrolPath.waypoints[character.WaypointIndex].waypointType == Waypoint.WaypointType.DEFAULT)
                {
                    character.WaypointIndex++;
                    character.MovePosition = character.patrolPath.waypoints[character.WaypointIndex].transform.position;
                    SetTask(moveTask);
                }
                // Wait at this waypoint for a time
                else if (character.patrolPath.waypoints[character.WaypointIndex].waypointType == Waypoint.WaypointType.WAIT_FOR_TIME)
                {
                    waitTask.waitTime = character.patrolPath.waypoints[character.WaypointIndex].waitTime;
                    SetTask(waitTask);
                    character.WaypointIndex++;
                }
                // Wait at this waypoint forever
                else if (character.patrolPath.waypoints[character.WaypointIndex].waypointType == Waypoint.WaypointType.WAIT_INDEFINITELY)
                {
                    waitTask.waitIndefinitely = true;
                    character.WaypointIndex++;
                    SetTask(waitTask);
                }
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting STATE_PATROL");

        base.ExitState();
    }
}
