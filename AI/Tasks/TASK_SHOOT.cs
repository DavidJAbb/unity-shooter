using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_SHOOT : BehaviourTask
{
    // Keep track of bullets fired and deal with reloading etc?
    // Reloading could just be fairly random, doesn't have to be exact - should serve gameplay

    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - TASK_SHOOT");
        // Return if there is no target
        if (character.CurrentTarget == null)
        {
            Debug.Log("TASK_SHOOT can't complete because there's no target.");
            StopTask();
        }
        else
        {
            base.StartTask();
        }
    }

    public override void UpdateTask()
    {
        // Aim at target
        // Fire gun
        character.gun.Fire();

        base.UpdateTask();
    }

    public override void StopTask()
    {
        base.StopTask();
    }
}
