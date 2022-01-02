/*
Player View script
Attach to the player camera (or container that holds the player camera)
Assumes a 'hands' child object to the player camera that holds the player weapons
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Setup")]
    public Transform hands;
    public Camera viewCam;

    [Header("Aim Down Sights")]
    public float aimFov = 40f;
    public float aimSpeed = 2; // speed of aiming

	private float _defaultFOV;
	private float _targetFOV;
    private float _speed;
    private Vector3 _startPos;
    private Vector3 _targetPos;

    private PlayerInventory _inventory;


    // Start is called before the first frame update
    void Start()
    {
        _inventory = GetComponent<PlayerInventory>();
        _defaultFOV = viewCam.fieldOfView;
        _startPos = hands.localPosition; // Default is 0.1, -0.175, 0.185
        _targetPos = _startPos;
    }

    public void DefaultView()
    {
        _targetFOV = _defaultFOV;
        _targetPos = _startPos;
        _speed = 2f;
    }

    public void AimDownSights(Vector3 pos)
    {
        _targetFOV = aimFov;
        _targetPos = pos;
        _speed = aimSpeed;
    }

	void FixedUpdate()
    {
		// Lerp between current position and target position
		hands.localPosition = Vector3.MoveTowards(hands.localPosition, _targetPos, Time.deltaTime * _speed);

		// Lerp view FOV
        if(viewCam.fieldOfView != _targetFOV)
        {
            viewCam.fieldOfView = Mathf.Lerp(viewCam.fieldOfView, _targetFOV, Time.deltaTime * _speed * 3);
        }
	}
}
