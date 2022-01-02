using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    private Character _character;

    private bool _isMoving;
    public bool IsMoving
    {
        get { return _isMoving; }
    }

    public bool IsTurning { get; set; }

    public void Init(Character character)
    {
        _character = character;
    }


    void FixedUpdate()
    {
        if (IsMoving == false && IsTurning == false)
            return;

        if (IsMoving && _character.Agent.hasPath) {
            // Vector3 feetPos = new Vector3(transform.position.x, transform.position.y-1, transform.position.z);
            // Vector3 pathEnd = agent.path.corners[agent.path.corners.Length-1];

            // Debug line from character to path end
            // Debug.DrawLine(feetPos, pathEnd, Color.yellow);
            // Debug ray forward
            // Vector3 forward = transform.TransformDirection(Vector3.forward) * 2;
            // Debug.DrawRay(feetPos, forward, Color.green);

            if(FinishedMove()) {
                // Reset rotation
                transform.rotation = Quaternion.LookRotation(transform.forward);
            }
            else {
                // Move and rotate
                Vector3 direction = DirectionToTarget(_character.Agent.steeringTarget);
                RotateToward(direction);
            }
        }

        if(IsTurning)
        {
            Vector3 direction = DirectionToTarget(_character.TurnTarget);
            RotateToward(direction);
        }
    }


    public bool FinishedMove()
    {
        // Check if we've reached the destination - TODO use this throughout!
        if (!_character.Agent.pathPending)
        {
            if (_character.Agent.remainingDistance <= _character.Agent.stoppingDistance)
            {
                if (!_character.Agent.hasPath || _character.Agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }


    public void WalkToPosition(Vector3 targetPos) {
        _character.Agent.enabled = true;
        _character.Agent.isStopped = false;
        _character.Agent.speed = _character.walkSpeed;
        _character.Agent.SetDestination(targetPos);
        _character.Animation.Walk(true);
        _isMoving = true;
    }

    
    public void RunToPosition(Vector3 targetPos) {
        _character.Agent.enabled = true;
        _character.Agent.isStopped = false;
        _character.Agent.speed = _character.runSpeed;
        _character.Agent.SetDestination(targetPos);
        _character.Animation.Run(true);
        _isMoving = true;
    }


    public void StopMoving() {
        _isMoving = false;
        _character.Agent.isStopped = true;
        _character.Animation.StopMoving();
    }


    public void RotateToward(Vector3 direction) {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        _character.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _character.turnSpeed);
    }


    private Vector3 DirectionToTarget(Vector3 targetPos) {
		// Gets a vector that points from our position to the target's.
		var heading = targetPos - _character.transform.position;
		var distance = heading.magnitude;
		var direction = heading / distance; // This is now the normalized direction.
		return direction;
    }
}
