/* Player Gun - covers all gun types inc. hitscan and projectile */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGun : PlayerWeapon
{
    //-------------Weapon Attributes---------------\\
    // All attributes are held in the weapon 'data' scriptable object - in the base class

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

    [Header("Projectiles")]
    [Tooltip("Check to fire projectiles - otherwise will use hitscan")]
    public bool firesProjectiles;
    private GameObject projectilePrefab;

    [Header("Audio")]
    public AudioClip fireSound;
    public AudioClip dryFireSound;
    public AudioClip reloadSound;

    [Header("Mods")]
    public BarrelMod barrelMod;


    //---------------Private References----------------\\
    private Player _player;
    private Camera _cam;
    private Animator _animator;
    private CameraShake _shake;
    private AudioSource _audio;
    private Vector3 _hitTarget;
    private float _nextShotTime;
    private float _msBetweenShots;
    private bool _triggerReleasedSinceLastAttack;
    private int _ammoInClip;

    // Tracers
    private int _tracerCounter; // Fire off tracers every 3 bullets and then every one of the last three bullets in a clip

    //---------------States----------------\\
    // Weapon States - Idle, Firing, Reloading
    public enum GunState
    {
        IDLE,
        FIRING,
        RELOADING
    }
    public GunState state;
    private readonly string[] _stateNames = { "Idle", "Firing", "Reloading" };

    public enum FireMode
    {
        SINGLE,
        BURST,
        AUTO
    }

    public override void Equip()
    {
        this.gameObject.SetActive(true);
        UpdateHUD();
    }


    public override void Init(Player player)
    {
        _player = player;
        _cam = player.playerCam;
        _animator = GetComponent<Animator>();
        //_shake = GetComponent<CameraShake>();
        _audio = GetComponent<AudioSource>();
        _hitTarget = new Vector3(_cam.pixelWidth * 0.5f, _cam.pixelHeight * 0.5f, 0);

        if(firesProjectiles)
        {
            projectilePrefab = Resources.Load("Projectile_Bullet") as GameObject;
        }

        // Set values.
        _msBetweenShots = 60 / data.rateOfFire;

        // Set defaults... (or the first fire input won't do anything!)
        _nextShotTime = Time.time;
        _triggerReleasedSinceLastAttack = true;

        // Ammo
        LoadClip();
    }


    public override void OnPrimaryHold()
    {
        SetState(GunState.FIRING);
        _triggerReleasedSinceLastAttack = false;
    }


    public override void OnPrimaryRelease()
    {
        SetState(GunState.IDLE);
        _triggerReleasedSinceLastAttack = true;
    }


    void Shoot()
    {
        if (CanShoot() && Time.time > _nextShotTime)
        {
            switch (data.fireMode)
            {
                case FireMode.SINGLE:
                    // Single Shot
                    if (!_triggerReleasedSinceLastAttack)
                        return;

                    if(firesProjectiles)
                    {
                        FireProjectile();
                    }
                    else
                    {
                        FireHitscan();
                    }
                    break;
                case FireMode.BURST:
                    if (!_triggerReleasedSinceLastAttack)
                        return;

                    // Fire off burst - not sure this does what we want? (This fires multiple bullets at the same time)
                    for (int i = 0; i < data.burstCount; i++)
                    {
                        if (firesProjectiles)
                        {
                            FireProjectile();
                        }
                        else
                        {
                            FireHitscan();
                        }
                    }
                    break;
                case FireMode.AUTO:
                    // Automatic - fire continuously
                    if (firesProjectiles)
                    {
                        FireProjectile();
                    }
                    else
                    {
                        FireHitscan();
                    }
                    break;
            }

            // Tracers
            _tracerCounter++;
            // Remove ammo
            _ammoInClip--;
            // Update the HUD
            UpdateHUD();
        }
    }

    public bool CanShoot()
    {
        // TODO is this right?
        if (_ammoInClip <= 0 && state != GunState.RELOADING)
        {
            return false;
        }
        return true;
    }


    public void FireHitscan()
    {
        // Fire a ray through the centre of the screen - but modify accuracy...
        Ray ray = _cam.ScreenPointToRay(ReturnAttackPointWithModifiedAccuracy());
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
                    damageableObject.TakeHit(new Hit((float)data.damagePerShot, hit.point, transform.forward));
                }

                // Check if object has a rigidbody and apply force...
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(ray.direction * 200f);
                }

                if(_tracerCounter == 2)
                {
                    // Bullet trail at hit point
                    SpawnBulletTrail(hit.point);
                }

                // Spawn particle FX and audio!
                GameManager.Instance.SpawnHitFX(hit);

                // Spawn distraction for enemies to hear...
                GameManager.Instance.SpawnDistraction(3, hit.point, 20f);
            }
        }
        else
        {
            if(_tracerCounter == 2)
            {
                // Bullet trail at some distant point in front of us...
                SpawnBulletTrail(ray.GetPoint(60));
            }
        }

        _animator.SetTrigger("shoot");
        _audio.clip = fireSound;
        _audio.Play();
        muzzleFlash.Play();

        GameManager.Instance.hud.ScaleCrosshair(data.kickBack);

        // Tracers
        if (_tracerCounter > 2)
        {
            _tracerCounter = 0;
        }
        // Last 3 bullets all fire tracers
        if(_ammoInClip < 4)
        {
            _tracerCounter = 2;
        }

        // Eject shell TODO should be animation driven?
        GameObject shellClone = Instantiate(shellPrefab, shellEjectTransform.position, shellEjectTransform.rotation) as GameObject;

        // Set the timer
        _nextShotTime = Time.time + _msBetweenShots / 1000;
    }
    
    
    public void FireProjectile()
    {
        // Fire a ray through the centre of the screen - NOTE not modifying accuracy here...
        Ray ray = _cam.ScreenPointToRay(ReturnAttackPointWithModifiedAccuracy());
        // Get a point along that ray...
        Vector3 forwardPos = ray.GetPoint(10);
        // Get the rotation that points to that point along the ray...
        Vector3 targetDirection = forwardPos - _cam.transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        // Spawn projectile - facing towards that rotation.
        GameObject projectileClone = Instantiate(projectilePrefab, _cam.transform.position, rotation) as GameObject;
        projectileClone.GetComponent<Projectile>().Init();

        _audio.clip = fireSound;
        _audio.Play();

        // Shake the weapon model and the view.
        //_shake.shakeAmount = data.weaponShakeAmount;
        //_shake.shakeDuration = data.weaponShakeDuration;

        // Muzzle flash.
        if (muzzleFlash != null)
            muzzleFlash.Play();
        // Eject shell.
        GameObject shellClone = Instantiate(shellPrefab, shellEjectTransform.position, shellEjectTransform.rotation) as GameObject;
        // Set the timer
        _nextShotTime = Time.time + _msBetweenShots / 1000;
    }
    

    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        Vector3 barrelEndPoint;
        if(barrelMod != null)
        {
            barrelEndPoint = barrelMod.barrelEnd.position;
        }
        else
        {
            barrelEndPoint = barrelEnd.position;
        }

        GameObject bulletTrailClone = Instantiate(bulletTrailPrefab, barrelEndPoint, Quaternion.identity) as GameObject;
        LineRenderer line = bulletTrailClone.GetComponent<LineRenderer>();
        line.SetPosition(0, barrelEndPoint);
        line.SetPosition(1, hitPoint);

        Destroy(bulletTrailClone, 1.0f);
    }

    // Realisitic clip stuff - ammo gets permanantly removed from inventory when loaded into gun.
    public void LoadClip()
    {
        int total = _player.inventory.GetTotalAmmo(data.ammoType);

        if (total >= data.clipSize)
        {
            _ammoInClip = data.clipSize;
            _player.inventory.RemoveAmmo(data.ammoType, data.clipSize);
        }
        else if(total < data.clipSize)
        {
            _ammoInClip = total;
            _player.inventory.RemoveAmmo(data.ammoType, total);
        }
        else if(total == 0)
        {
            Debug.Log("No ammo for this weapon in inventory!");
        }

        _tracerCounter = 0;
    }


    public override void Reload()
    {
        // Can only reload if gun is idle
        if(state == GunState.IDLE)
        {
            SetState(GunState.RELOADING);
        }
    }


    public override void Unequip() {
        this.gameObject.SetActive(false);
    }


    public override void UpdateHUD() {
        GameManager.Instance.hud.UpdateAmmo(_ammoInClip, _player.inventory.GetTotalAmmo(data.ammoType));
    }

    #region States
    // Inherited classes have to have Idle, Firing, and Reloading functions...
    public void SetState(GunState newState) {
        // If new state is the same as current state - do nothing.
        if (newState == state || _canChangeState == false)
            return;

        // Set the new state.
        state = newState;
        // Start the new state coroutine.
        StartCoroutine(_stateNames[(int)newState]);
    }


    IEnumerator Idle() {
        while (state == GunState.IDLE) {
            yield return null;
        }
    }

    // Firing
    IEnumerator Firing() {
        if(_ammoInClip == 0)
        {
            _audio.clip = dryFireSound;
            _audio.Play();
            print("Out of ammo, you have to reload!");
            yield return null;
        }
        else if(_ammoInClip > 0)
        {
            while (state == GunState.FIRING)
            {
                Shoot();
                yield return null;
            }
        }
    }

    private bool _canChangeState = true;

    // Reload
    IEnumerator Reloading() {

        _canChangeState = false;

        print("Weapon is reloading.");

        _animator.SetTrigger("reload");

        // Play sound
        _audio.clip = reloadSound;
        _audio.Play();

        while (state == GunState.RELOADING) {
            // Wait for the reload time.
            yield return new WaitForSeconds(data.reloadTime);
            // Spawn empty clip.
            // TODO
            // Load another clip.
            LoadClip();
            UpdateHUD();
            // Finish reloading - move to idle state.
            _canChangeState = true;
            SetState(GunState.IDLE);

            yield return null;
        }
    }
    #endregion

    // Modify the accuracy of the hit point.
    // IDEA - Hit circle grows while trigger is held down so full auto gets progressively less accurate?
    // Maybe camera shake also gets worse the longer the trigger is held?
    public Vector3 ReturnAttackPointWithModifiedAccuracy() {
        Vector2 accuracyModifier = UnityEngine.Random.insideUnitCircle * data.accuracy;
        Vector3 modifiedAttackPos = _hitTarget;
        modifiedAttackPos.x += accuracyModifier.x;
        modifiedAttackPos.y += accuracyModifier.y;
        return modifiedAttackPos;
    }
}

[System.Serializable]
public class Hit
{
    public float damage;
    public Vector3 point;
    public Vector3 direction;

    public Hit(float hitDamage, Vector3 hitPoint, Vector3 hitDirection)
    {
        this.damage = hitDamage;
        this.point = hitPoint;
        this.direction = hitDirection;
    }
}
