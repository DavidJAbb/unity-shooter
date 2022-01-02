using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;

public class LiftPlatform :  Platform
{
	public Transform[] nodeTransforms;
	public bool IsMoving { get; set; }

	int _curIndex;
	Vector3 startingPos;
	Vector3 targetPos;

	public float lerpTime = 5f;
	float currentLerpTime;


    void FixedUpdate()
	{
		if(IsMoving)
        {
			currentLerpTime += Time.deltaTime;
			RigidbodyComponent.Move(CalculatePosition());

			if(currentLerpTime > lerpTime)
            {
				currentLerpTime = 0;
				IsMoving = false;
			}
		}
	}


	public void MoveToPosition(int index)
    {
		if (IsMoving || _curIndex == index)
			return;

		_curIndex = index;
		startingPos = transform.position;
		targetPos = nodeTransforms[_curIndex].position;
		IsMoving = true;
	}


	Vector3 CalculatePosition()
	{
		float perc = currentLerpTime / lerpTime;
		Vector3 position = Vector3.Lerp(startingPos, targetPos, perc);
		return position;
	}
}
