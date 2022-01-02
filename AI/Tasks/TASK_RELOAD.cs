using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_RELOAD : BehaviourTask
{
    public float reloadTime;
    float _timer;


    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - RELOAD");

        _timer = reloadTime;
        // character.Animation.StopAiming(); // TODO Need to separate aiming IK and aiming stance in animator!!!
        character.Movement.IsTurning = false;

        character.Animation.Reload();
        character.gun.Reload();

        base.StartTask();
    }

    public override void UpdateTask()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            character.BulletsInClip = 20;
            character.Debug_UpdateStatusText();
            StopTask();
        }

        base.UpdateTask();
    }


    public override void StopTask()
    {
        base.StopTask();
    }
}
