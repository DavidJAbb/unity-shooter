using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_DIE : BehaviourTask
{
    public float animationLength = 0f;
    private float _timer = 0;

    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - TASK_DIE");

        _timer = 0f;

        character.Animation.Die();

        base.StartTask();
    }

    public override void UpdateTask()
    {
        _timer += Time.deltaTime;

        // Let the animation finish...
        if (_timer > animationLength)
        {
            StopTask();
        }
    }

    public override void StopTask()
    {
        _timer = 0;
        base.StopTask();
    }
}