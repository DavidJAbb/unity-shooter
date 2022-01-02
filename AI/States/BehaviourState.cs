using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI Behaviour State base class
public class BehaviourState: MonoBehaviour
{
    public int priority;

    [HideInInspector]
    public ExecutionState executionState;

    [HideInInspector]
    public Character character;

    // public BehaviourSequence[] Sequences { get; set; } // An array of all possible sequences
    public BehaviourTask CurrentTask { get; set; }


    public virtual void Init(Character character)
    {
        this.character = character;
        executionState = ExecutionState.NONE;
    }

    public virtual bool ConditionsMet()
    {
        return false;
    }

    public virtual void EnterState()
    {
        executionState = ExecutionState.ACTIVE;
    }

    public virtual void UpdateState()
    {
        if (CurrentTask != null)
        {
            if (CurrentTask.executionState == ExecutionState.ACTIVE)
            {
                CurrentTask.UpdateTask();
            }
        }
    }

    public virtual void ExitState()
    {
        if (CurrentTask != null && CurrentTask.executionState == ExecutionState.ACTIVE)
        {
            CurrentTask.StopTask();
        }

        character.Behaviour.CheckDecisionNextFrame = true;
        executionState = ExecutionState.COMPLETED;
    }

    
    public void SetTask(BehaviourTask newTask)
    {
        if (CurrentTask != null)
        {
            if (newTask == CurrentTask && CurrentTask.executionState == ExecutionState.ACTIVE)
                return;

            CurrentTask.StopTask();
        }

        CurrentTask = newTask;
        CurrentTask.StartTask();
        character.Debug_UpdateStatusText(); // Debug
    }

    // REMOVE
    public BehaviourSequence CurrentSequence { get; set; }
    public void SetSequence(BehaviourSequence newSequence)
    {
        // Do nothing...
    }
}
