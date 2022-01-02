using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_RUN_TO_COVER : BehaviourTask
{
    private CoverPosition _coverPosition;

    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - RUN TO COVER");
        base.StartTask();

        character.Animation.AnimationAim(false); // Move out of aim stance

        // Get cover position
        _coverPosition = character.combatZone.BestCoverPositionFacingTarget(character.transform, character.CurrentTarget.body.transform);

        if (_coverPosition != null)
        {
            _coverPosition.isReserved = true; // Reserve the position to stop others trying to use it
            // Set cover pos as move pos and run to it
            character.MovePosition = _coverPosition.transform.position;
            character.Movement.RunToPosition(_coverPosition.transform.position);
        }
        else
        {
            Debug.Log("TASK_RUN_TO_COVER: No cover position available");
            StopTask();
        }
    }

    public override void UpdateTask()
    {
        if (character.Movement.FinishedMove())
        {
            character.InCover = true;
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
