using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Door script - attach to the 'hinge' object with the door model parented to that. Door rotates around the transform.
[SelectionBase]
[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    public enum DoorType
    {
        SWINGING,
        SLIDING
    }
    public DoorType doorType;

    [Header("Swing Doors")]
    public float doorOpenAngle = 90.0f; // + opens inwards / - opens outwards
    public bool openBothWays = true; // Swing away from player no matter which side player opens from
    private float _closeAngle;

    [Header("Sliding Doors")]
    public float openPosX = 1.45f; // The distance to move in the local X. 0 should be closed pos.

    [Header("Timing")]
    public float openTime = 2f;
    public float closeTime = 2f;

    [Header("Lock Setup")]
    public bool isLocked;
    public LockBase doorLock;

    [Header("Audio")]
    public AudioClip openSound;
    private AudioSource _audioSource;

    bool _isOpen;
    bool _isMoving;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _isMoving = false;
        _isOpen = false;
    }


    public void TryOpen(bool isInner)
    {
        if (_isMoving)
            return;

        if (isLocked)
        {
            doorLock.TryUnlock();
        }
        else if (!isLocked)
        {
            if (_isOpen)
            {
                CloseDoor();

            }
            else if (!_isOpen)
            {
                OpenDoor(isInner);
            }
        }
    }


    void OpenDoor(bool isInner)
    {
        _isMoving = true;

        switch(doorType)
        {
            case DoorType.SWINGING:
                if (openBothWays && isInner)
                {
                    LeanTween.rotateAroundLocal(gameObject, transform.InverseTransformVector(transform.up), -doorOpenAngle, openTime).setEaseOutExpo().setOnComplete(FinishOpen);
                    _closeAngle = doorOpenAngle;
                }
                else
                {
                    LeanTween.rotateAroundLocal(gameObject, transform.InverseTransformVector(transform.up), doorOpenAngle, openTime).setEaseOutExpo().setOnComplete(FinishOpen);
                    _closeAngle = -doorOpenAngle;
                }
                break;
            case DoorType.SLIDING:
                LeanTween.moveLocalX(gameObject, openPosX, openTime).setOnComplete(FinishOpen);
                break;
        }

        _audioSource.clip = openSound;
        _audioSource.Play();
    }

    void CloseDoor()
    {
        _isMoving = true;

        switch (doorType)
        {
            case DoorType.SWINGING:
                LeanTween.rotateAroundLocal(gameObject, transform.InverseTransformVector(transform.up), _closeAngle, closeTime).setOnComplete(FinishClose);
                break;
            case DoorType.SLIDING:
                LeanTween.moveLocalX(gameObject, 0f, closeTime).setOnComplete(FinishClose);
                break;
        }

        _audioSource.clip = openSound;
        _audioSource.Play();
    }

    void FinishOpen()
    {
        _isMoving = false;
        _isOpen = true;
    }

    void FinishClose()
    {
        _isMoving = false;
        _isOpen = false;
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
        // TODO play unlock sound
        Debug.Log("Door unlocked!");
    }
}
