using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_INVESTIGATE : BehaviourState
{
    BehaviourSequence investigateSequence;
    Distraction curDistraction;

    public override void Init(Character character)
    {
        // Set up TASKS
        BehaviourTask waitTask = new TASK_WAIT ();
        BehaviourTask turnTask = new TASK_TURN_TO_POSITION();
        BehaviourTask moveTask = new TASK_MOVE_TO_POSITION();

        // Set up SEQUENCES
        investigateSequence = new BehaviourSequence();
        investigateSequence.Init(character, new BehaviourTask[] { turnTask, waitTask, moveTask, waitTask });

        base.Init(character);
    }

    public override bool ConditionsMet()
    {
        if (character.Distraction != null)
        {
            return true;
        }

        return false;
    }

    public override void EnterState()
    {
        Debug.Log("Entered STATE_INVESTIGATE");

        curDistraction = character.Distraction;

        character.MovePosition = character.Distraction.transform.position; // Set the move position
        character.TurnTarget = character.Distraction.transform.position; // Set the turn position
        SetSequence(investigateSequence); // Start the sequence

        character.Animation.SetStance(CharacterAnimation.Stance.COMBAT);

        base.EnterState();
    }

    public override void UpdateState()
    {
        // If enemy has a new distraction - restart with that one...
        if(curDistraction != character.Distraction)
        {
            CurrentSequence.StopSequence(); // Stop the current sequence
            EnterState(); // Restart with a new distraction to investigate
            return;
        }

        if (CurrentSequence.executionState == ExecutionState.COMPLETED)
        {
            ExitState();
        }

        base.UpdateState();
    }

    public override void ExitState()
    {
        Debug.Log("Exiting STATE_INVESTIGATE");

        if(curDistraction == character.Distraction)
        {
            character.ClearDistraction(); // Remove the distraction that has been investigated.
        }

        curDistraction = null;

        base.ExitState();
    }
}
