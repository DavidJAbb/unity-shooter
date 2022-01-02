using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_IDLE : BehaviourState
{
    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override bool ConditionsMet()
    {
        return true;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
