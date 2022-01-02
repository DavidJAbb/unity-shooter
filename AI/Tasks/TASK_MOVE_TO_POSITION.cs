using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_MOVE_TO_POSITION : BehaviourTask
{
    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - MOVE TO POSITON");
        base.StartTask();
        character.Movement.WalkToPosition(character.MovePosition);
    }

    public override void UpdateTask()
    {
        if (character.Movement.FinishedMove())
        {
            StopTask();
        }
        base.UpdateTask();
    }

    public override void StopTask()
    {
        character.Movement.StopMoving();
        base.StopTask();
    }
}
