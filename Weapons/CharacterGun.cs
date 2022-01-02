using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGun : MonoBehaviour
{
    //-------------Weapon Attributes---------------\\
    public float rateOfFire = 0.8f; // 0.8f for machine gun
    public float accuracy = 10f;
    public float damagePerShot = 10f;

    //---------------Model Setup----------------\\
    [Header("Prefab Setup")]
    [Tooltip("The position that the bullet will come from.")]
    public Transform barrelEnd;
    [Tooltip("The muzzle flash particle system.")]
    public ParticleSystem muzzleFlash;
    [Tooltip("The shell ejection prefab.")]
    public GameObject shellPrefab;
    public Transform shellEjectTransform;
    public GameObject bulletTrailPrefab;

    [Header("Audio")]
    public AudioClip fireSound;
    public AudioClip dryFireSound;
    public AudioClip reloadSound;

    //---------------Private References----------------\\
    private AudioSource _audio;
    private Vector3 _hitTarget;

    // Tracers
    private int _tracerCounter; // Fire off tracers every third bullet


    public void Init()
    {
        _audio = GetComponent<AudioSource>();
    }


    public void Reload()
    {
        _audio.clip = reloadSound;
        _audio.Play();
    }


    public void Update()
    {
        // Debug.DrawRay(barrelEnd.position, barrelEnd.forward, Color.red);
        Ray ray = new Ray(barrelEnd.position, barrelEnd.forward);
        Debug.DrawLine(barrelEnd.position, ray.GetPoint(30), Color.red);
    }


    public void Fire()
    {
        Ray aimRay = new Ray(barrelEnd.position, barrelEnd.forward);
        Vector3 aimPos = aimRay.GetPoint(60);
        Vector3 directionToAimPos = ReturnAttackPointWithModifiedAccuracy(aimPos) - barrelEnd.position;
        Ray ray = new Ray(barrelEnd.position, directionToAimPos);

        RaycastHit hit;

        // Check if ray hits anything
        if (Physics.Raycast(ray, out hit, 1000, GameManager.Instance.shootableLayers))
        {
            if (hit.collider != null)
            {
                // Check if the hit object is a damageable object...
                IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
                if (damageableObject != null)
                {
                    // Apply damage
                    damageableObject.TakeHit(new Hit(damagePerShot, hit.point, transform.forward));
                }

                if (_tracerCounter == 2)
                {
                    // Bullet trail at hit point
                    SpawnBulletTrail(hit.point);
                }

                // Spawn particle FX
                GameManager.Instance.SpawnHitFX(hit);
            }
        }
        else
        {
            if (_tracerCounter == 2)
            {
                // Bullet trail at some distant point in front of us...
                SpawnBulletTrail(ray.GetPoint(60));
            }
        }

        // Audio
        _audio.clip = fireSound;
        _audio.Play();

        // Tracers
        _tracerCounter++;
        if (_tracerCounter > 2)
        {
            _tracerCounter = 0;
        }

        // Muzzle flash.
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Eject shell.
        GameObject shellClone = Instantiate(shellPrefab, shellEjectTransform.position, shellEjectTransform.rotation) as GameObject;
    }


    private void SpawnBulletTrail(Vector3 endPoint)
    {
        GameObject bulletTrailClone = Instantiate(bulletTrailPrefab, barrelEnd.position, Quaternion.identity) as GameObject;
        LineRenderer line = bulletTrailClone.GetComponent<LineRenderer>();
        line.SetPosition(0, barrelEnd.position);
        line.SetPosition(1, endPoint);

        Destroy(bulletTrailClone, 1.0f);
    }


    // Modify the accuracy of the hit point
    // TODO Hit circle grows while trigger is held down so full auto gets progressively less accurate? Or make it more accurate over time?
    public Vector3 ReturnAttackPointWithModifiedAccuracy(Vector3 aimPoint)
    {
        Vector3 accuracyModifier = Random.insideUnitSphere * accuracy;
        Vector3 modifiedAttackPos = aimPoint;
        modifiedAttackPos.x += accuracyModifier.x;
        modifiedAttackPos.y += accuracyModifier.y;
        modifiedAttackPos.z += accuracyModifier.z;
        return modifiedAttackPos;
    }
}
