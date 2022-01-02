/*
Weapon Sway - produces old school shooter weapon sway effect
This script relies on the Character Controller Pro asset to get the character velocity...
...but the principle would be the same for a different character controller
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.Utilities;
using Lightbug.CharacterControllerPro;


public class WeaponSway : MonoBehaviour
{
    public bool allowSway;
    public Lightbug.CharacterControllerPro.Core.CharacterActor characterActor;
    
    private float _speed;
    private float _radius;
    private float _phase;
    private Vector3 _targetPos;
    private Vector3 _startingPos;
    private float lerpTime = 1f;
    private float currentLerpTime = 0f;
    private Vector3 _characterVelocity;
    private float _adjustedVelocity;

    public enum SwayState
    {
        Idle,
        Run,
        Sprint
    }
    private SwayState swayState;


    void Start()
    {
        _startingPos = transform.localPosition;
        _targetPos = _startingPos;

        allowSway = true;
    }


    private void FixedUpdate()
    {
        if(allowSway)
        {
            _characterVelocity = characterActor.Velocity;
            _characterVelocity.y = 0;
            _adjustedVelocity = _characterVelocity.magnitude;

            if (_adjustedVelocity > 0 && _adjustedVelocity <= 10)
            {
                SetSwayState(SwayState.Run);
            }

            if (_adjustedVelocity > 10)
            {
                SetSwayState(SwayState.Sprint);
            }

            if (_adjustedVelocity <= 0)
            {
                SetSwayState(SwayState.Idle);
            }
        }
        else
        {
            SetSwayState(SwayState.Idle);
        }

        if(swayState == SwayState.Idle)
        {
            // If the current position is different to the starting position - lerp to that position.
            //Lerp timer - increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float perc = currentLerpTime / lerpTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startingPos, perc);
        }
        else
        {
            // Run Animation
            // Lerp calculation - a nice figure of 8 like Quake.
            _phase += Time.deltaTime * _speed;
            _targetPos.x = Mathf.Sin(_phase) * _radius * 2.5f;
            _targetPos.y = Mathf.Sin(_phase * 2f) * _radius;
            // Update the cam position
            transform.localPosition = _targetPos;
        }
    }


    void SetSwayState(SwayState newState)
    {
        if (newState == swayState)
            return;

        switch(newState)
        {
            case SwayState.Idle:
                _speed = 2f;
                _radius = 0.0025f;

                lerpTime = 1f;
                currentLerpTime = 0f;
                break;
            case SwayState.Run:
                // Set values
                _speed = 6f;
                _radius = 0.008f;
                // Reset values
                if(swayState == SwayState.Idle)
                {
                    _phase = 0f;
                    _targetPos = _startingPos;
                }
                break;
            case SwayState.Sprint:
                // Set values
                _speed = 8f;
                _radius = 0.010f;
                // Reset values
                if (swayState == SwayState.Idle)
                {
                    _phase = 0f;
                    _targetPos = _startingPos;
                }
                break;
        }
        swayState = newState;
    }
}
