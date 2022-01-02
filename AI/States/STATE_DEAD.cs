using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_DEAD : BehaviourState
{
    [Range(0, 30)]
    public float dieAnimationLength = 6f;

    TASK_DIE dieTask;

    public override void Init(Character character)
    {
        dieTask = new TASK_DIE();
        dieTask.Init(character);
        dieTask.animationLength = dieAnimationLength;

        base.Init(character);
    }

    public override bool ConditionsMet()
    {
        if (character.Health <= 0)
            return true;

        return false;
    }

    public override void EnterState()
    {
        character.IsDead = true;
        character.Animation.AimIK(false);
        character.Animation.AnimationAim(false);
        character.Animation.DisableAllAnimationRigs();
        character.DisableCharacterCollision();
        character.DisableSight(); // Stop sight checks and look controller (IK)
        character.Movement.StopMoving(); // Stop moving
        character.Movement.enabled = false; // Disable movement
        character.Agent.enabled = false; // Disable NavMeshAgent
        character.DisableWeapon();
        // character.SpawnLoot();

        SetTask(dieTask); // Plays the die animation

        base.EnterState();
    }

    public override void UpdateState()
    {
        if (CurrentTask.executionState == ExecutionState.COMPLETED)
        {
            ExitState();
        }

        base.UpdateState();
    }

    public override void ExitState()
    {
        character.Animation.DisableAnimator();
        base.ExitState();
    }
}