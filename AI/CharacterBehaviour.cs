using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public Character Character { get; set; }
    public BehaviourState[] States { get; set; }
    public BehaviourState CurrentState { get; set; }
    public bool CheckDecisionNextFrame { get; set; }

    public BehaviourState defaultState;

    private BehaviourState _stateToChose;
    private BehaviourState _prevState;


    public void Init(Character character)
    {
        Character = character;

        States = GetComponentsInChildren<BehaviourState>();

        foreach (BehaviourState state in States)
        {
            state.Init(Character);
        }

        CheckDecisionNextFrame = true; // Make sure it does the initial check...
    }


    public void UpdateBehaviours()
    {
        if (CurrentState != null && CurrentState.executionState == ExecutionState.ACTIVE)
        {
            CurrentState.UpdateState();
        }
    }


    // Called when inputs change or when a state has run through its current sequences...
    public void CheckDecision()
    {
        // If the current state is null then just start the default state...
        if(CurrentState == null)
        {
            CurrentState = defaultState;
            CurrentState.EnterState();
            Character.Debug_UpdateStatusText();
            return;
        }

        _stateToChose = defaultState;

        foreach (BehaviourState state in States)
        {
            // If state's conditions are met and priority is higher than state to compare - then this becomes new state...
            if (state.ConditionsMet() && state.priority > _stateToChose.priority)
            {
                _stateToChose = state;
            }
        }

        // Set previous and new current states
        if(_stateToChose != CurrentState)
        {
            _prevState = CurrentState;
            _prevState.ExitState();
            CurrentState = _stateToChose;
            CurrentState.EnterState();
            Character.Debug_UpdateStatusText();
            // Debug.Log($"Entering new state: {CurrentState}");
        }

        CheckDecisionNextFrame = false;
    }
}
