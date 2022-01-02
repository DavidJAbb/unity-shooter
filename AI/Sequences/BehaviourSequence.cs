using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSequence
{
    public ExecutionState executionState;
    private BehaviourTask[] _tasks;
    public BehaviourTask CurrentTask { get; set; }
    private int _index;
    // private Character _character;

    public void Init(Character character, BehaviourTask[] tasks)
    {
        // _character = character;
        _tasks = tasks;

        foreach (BehaviourTask task in _tasks)
        {
            task.Init(character);
        }
        executionState = ExecutionState.NONE;
    }

    public void StartSequence()
    {
        CurrentTask = _tasks[_index];
        CurrentTask.StartTask();
        executionState = ExecutionState.ACTIVE;
    }

    public void UpdateSequence()
    {
        if (CurrentTask == null)
            return;

        if (executionState == ExecutionState.ACTIVE)
        {
            if (CurrentTask.executionState == ExecutionState.ACTIVE)
            {
                CurrentTask.UpdateTask();
            }

            if (CurrentTask.executionState == ExecutionState.COMPLETED)
            {
                _index++;

                if (_index > _tasks.Length - 1)
                {
                    StopSequence();
                }
                else if (_index <= _tasks.Length - 1)
                {
                    CurrentTask = _tasks[_index];
                    CurrentTask.StartTask();
                }
            }
        }
    }

    public void StopSequence()
    {
        // Stop any running tasks...
        if (CurrentTask.executionState == ExecutionState.ACTIVE)
        {
            CurrentTask.StopTask();
        }

        executionState = ExecutionState.COMPLETED;
        _index = 0;
        Debug.Log("Sequence completed");
    }
}
