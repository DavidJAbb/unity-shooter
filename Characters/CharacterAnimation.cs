using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimation : MonoBehaviour
{
    public Animator Animator { get; set; }

    [Header("Animation Rigging")]
    public Rig lookRig;
    public Transform lookTarget;

    public Rig aimRig;
    public Transform aimTarget;
    public Vector3 aimDefaultPos; // The local default position

    public bool IsAiming { get; set; }
    public bool IsLooking { get; set; }

    // Should most of this happen in the Tasks?

    public enum Stance
    {
        IDLE,
        COMBAT,
        CROUCH
    }
    public Stance CharacterStance { get; }


    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        if(aimTarget != null)
        {
            aimDefaultPos = aimTarget.localPosition;
        }

        SetStance(Stance.IDLE); // Default starting stance
    }

    public void Walk(bool value)
    {
        Animator.SetBool("isWalking", value);
    }

    public void Run(bool value)
    {
        Animator.SetBool("isRunning", value);
    }

    public void StopMoving()
    {
        Animator.SetBool("isWalking", false);
        Animator.SetBool("isRunning", false);
    }

    public void SetStance(Stance newStance)
    {
        if (newStance == CharacterStance)
            return;

        switch(newStance)
        {
            case Stance.IDLE:
                Animator.SetBool("inCrouchingStance", false);
                Animator.SetBool("inCombatStance", false);
                break;
            case Stance.COMBAT:
                Animator.SetBool("inCrouchingStance", false);
                Animator.SetBool("inCombatStance", true);
                break;
            case Stance.CROUCH:
                Animator.SetBool("inCombatStance", false);
                Animator.SetBool("inCrouchingStance", true);
                break;
        }
    }

    public void AnimationAim(bool value)
    {
        Animator.SetBool("isAiming", value);
    }

    public void AimIK(bool value)
    {
        IsAiming = value;
    }

    public void Throw()
    {
        Animator.SetTrigger("Throw");
    }

    public void ShootOnce()
    {
        Animator.SetTrigger("ShootSingle");
    }

    public void ShootBurst()
    {
        Animator.SetTrigger("ShootBurst");
    }

    public void Reload()
    {
        Animator.SetTrigger("Reload");
    }

    // From any state
    public void Die()
    {
        // TODO logic to pick animation based on direction of attack
        Animator.SetBool("isDead", true);
    }

    public void DisableAllAnimationRigs()
    {
        lookRig.weight = 0;
        aimRig.weight = 0;
    }

    public void DisableAnimator()
    {
        Animator.enabled = false;
    }
}
