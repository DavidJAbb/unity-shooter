using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK_TURN_TO_POSITION : BehaviourTask
{
    private Vector3 _targetDirection;
    private Quaternion _startRotation;
    private Quaternion _endRotation;

    float _lerpTime = 0.5f;
    float _currentLerpTime;


    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void StartTask()
    {
        Debug.Log("NEW TASK - TASK_TURN_TO_POSITION");
        _targetDirection = character.TurnTarget - character.transform.position;
        _startRotation = character.transform.rotation;
        _endRotation = Quaternion.LookRotation(new Vector3(_targetDirection.x, 0, _targetDirection.z));
        _currentLerpTime = 0;

        base.StartTask();
    }

    public override void UpdateTask()
    {
        // Lerp like a pro...
        _currentLerpTime += Time.deltaTime;
        if (_currentLerpTime > _lerpTime)
        {
            _currentLerpTime = _lerpTime;
        }

        float perc = _currentLerpTime / _lerpTime;
        character.transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, perc);
        if (perc == 1)
        {
            Debug.Log("Finished rotating to target rotation!");
            StopTask();
        }

        base.UpdateTask();
    }

    public override void StopTask()
    {
        base.StopTask();
    }
}
