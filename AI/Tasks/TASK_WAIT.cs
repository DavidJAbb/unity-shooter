using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_WAIT : BehaviourTask
{
    public float waitTime;
    public bool waitIndefinitely;
    private float _timer = 0;

    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - WAIT");
        _timer = 0;
        base.StartTask();
    }

    public override void UpdateTask()
    {
        if (waitIndefinitely)
            return;

        _timer += Time.deltaTime;
        if (_timer >= waitTime)
        {
            StopTask();
        }

        base.UpdateTask();
    }

    public override void StopTask()
    {
        _timer = 0;
        waitTime = 0;
        waitIndefinitely = false;
        base.StopTask();
    }
}
