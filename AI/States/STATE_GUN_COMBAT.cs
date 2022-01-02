using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_GUN_COMBAT : BehaviourState
{
    [Range(0, 5)]
    public float aimTime = 0.5f;
    [Range(0, 5)]
    public float shootOnceTime = 0.25f;
    [Range(0, 5)]
    public float shootBurstTime = 1f;
    [Range(0, 10)]
    public float reloadTime = 4.75f;

    // Tasks - AimStanding, ShootOnceStanding, ShootBurstStanding, Reload, ThrowGrenade
    TASK_STAND_AND_SHOOT shootStandingTask;
    TASK_RELOAD reloadTask;
    TASK_RUN_TO_COVER runToCoverTask;


    public override void Init(Character character)
    {
        // Set up tasks
        shootStandingTask = new TASK_STAND_AND_SHOOT();
        shootStandingTask.Init(character);
        shootStandingTask.aimTime = aimTime;
        shootStandingTask.shootTime = shootOnceTime;

        reloadTask = new TASK_RELOAD();
        reloadTask.Init(character);
        reloadTask.reloadTime = reloadTime;

        runToCoverTask = new TASK_RUN_TO_COVER();
        runToCoverTask.Init(character);

        base.Init(character);
    }

    public override bool ConditionsMet()
    {
        if(character.CurrentTarget != null || character.UnderFire)
        {
            return true;
        }
        return false;
    }

    public override void EnterState()
    {
        character.InCombat = true;
        character.Animation.SetStance(CharacterAnimation.Stance.COMBAT);
        SetTask(shootStandingTask);

        base.EnterState();
    }

    // Should be doing more in here. Convert Run to Cover to just Run - and decide if we want to be in cover here...
    // If X, Get Cover Position, Run to it, Shoot from cover, standing or crouching...

    public override void UpdateState()
    {
        base.UpdateState();

        // TODO Three stages...
        // 1. Determine best position based on factors like target position and range, health etc. - chase, flee, find cover, advance
        // - If target is running away - chase, when in range - shoot, if target is firing on us - shoot back or get in cover
        // - If target is in cover and / or not moving much - throw grenade or advance...
        // 2. Move - move(walk, run, sprint)
        // 3. Attack - shoot, throw grenade, reload
        // Then random decision - move position - should be low-ish probability

        // If not already in cover and health is low - get to cover
        if(!character.InCover && character.Health < character.maxHealth * 0.5f)
        {
            SetTask(runToCoverTask);
        }

        // If under fire TODO or need to reload - get to cover then reload
        if (CurrentTask == shootStandingTask && CurrentTask.executionState == ExecutionState.COMPLETED && character.BulletsInClip == 0)
        {
            SetTask(reloadTask);
        }

        // If reloaded OR if finished running to cover
        if (CurrentTask == reloadTask && CurrentTask.executionState == ExecutionState.COMPLETED || CurrentTask == runToCoverTask && CurrentTask.executionState == ExecutionState.COMPLETED)
        {
            SetTask(shootStandingTask);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}