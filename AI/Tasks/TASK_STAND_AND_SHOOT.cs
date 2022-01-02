using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_STAND_AND_SHOOT : BehaviourTask
{
    public float aimTime;
    public float shootTime;

    float _timer;
    bool _shootNextFrame;
    bool _isShooting;


    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - STAND AND SHOOT");

        _timer = aimTime; // Initial aim / turn time
        _isShooting = false;
        _shootNextFrame = false;

        character.Movement.IsTurning = true; // Turn to face target
        character.Animation.AimIK(true);
        character.Animation.AnimationAim(true);

        base.StartTask();
    }

    public override void UpdateTask()
    {
        // Count shots fired and set reload
        if (character.BulletsInClip <= 0)
        {
            // End task?
            StopTask();
        }
        else
        {
            // Turn / Aim at target
            character.TurnTarget = character.CurrentTarget.body.position;

            // Shoot
            if (_shootNextFrame)
            {
                // Shoot one shot
                character.Animation.ShootOnce();
                character.gun.Fire();
                character.BulletsInClip--;
                _timer = shootTime;
                _shootNextFrame = false;
                _isShooting = true;

                character.Debug_UpdateStatusText();
            }

            // Shot timer
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                if (_isShooting)
                {
                    _timer = shootTime; // Add shoot time
                    _isShooting = false;
                }
                else
                {
                    _shootNextFrame = true;
                }
            }
        }

        base.UpdateTask();
    }


    public override void StopTask()
    {
        character.Movement.IsTurning = false; // Turn to face target
        character.Animation.AimIK(false);

        base.StopTask();
    }
}
