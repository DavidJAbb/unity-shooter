/*
Player script
I'm using Character Controller Pro for movement but it's mostly applicable to any first person game
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : CharacterBase
{
    [Header("Player")]
    public Volume postProcessVolume;
    public float interactDistance = 1.0f;
    public LayerMask interactLayerMask;
    public Transform handsTransform;

    [Header("FPS Camera")]
    public Camera playerCam;
    public GameObject cameraObject;

    [Header("CCP setup")]
    public Lightbug.CharacterControllerPro.Core.CharacterActor characterActor;
    public Lightbug.CharacterControllerPro.Demo.Camera3D camera3D;
    public LayerMask floorMask;

    // Player Sub-Components
    [HideInInspector]
    public PlayerInventory inventory;
    [HideInInspector]
    public PlayerView view;
    [HideInInspector]
    public WeaponSway weaponSway;

    // Input
    public bool AllowWeaponInput { get; set; }
    public bool AllowInput { get; set; }

    // private Danger _dangerEffect;
    private ColorAdjustments _colorAdjustments;
    private float _damageDuration = 1.25f;
    private float _deathAnimDuration = 1f;


    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        view = GetComponent<PlayerView>();
        weaponSway = GetComponentInChildren<WeaponSway>();

        // postProcessVolume.profile.TryGet<Danger>(out _dangerEffect);
        postProcessVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);

        // Init Health
        Init(100);
    }


    void Start()
    {
        AllowWeaponInput = true;
        AllowInput = true;
    }


    void Update()
    {
        if(AllowInput)
        {
            // WEAPON INTERACTIONS
            if (AllowWeaponInput && inventory.curWeapon != null)
            {
                // Start attack
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    inventory.curWeapon.OnPrimaryHold();
                }

                // Stop attack
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    inventory.curWeapon.OnPrimaryRelease();
                }

                // Aim Down Sights - Start
                if (Input.GetKeyDown(KeyCode.I))
                {
                    view.AimDownSights(inventory.curWeapon.data.aimDownSightsPosition);
                    weaponSway.allowSway = false;
                }

                // Aim Down Sights - Stop
                if (Input.GetKeyUp(KeyCode.I))
                {
                    view.DefaultView();
                    weaponSway.allowSway = true;
                }

                // Reload
                if (Input.GetKeyUp(KeyCode.R))
                {
                    inventory.curWeapon.Reload();
                }
            }

            // Press E to interact
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = playerCam.ScreenPointToRay(new Vector3(playerCam.pixelWidth * 0.5f, playerCam.pixelHeight * 0.5f, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, interactDistance, interactLayerMask))
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
                }
            }

            // Equip melee weapon
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                inventory.EquipWeapon(WeaponType.MELEE);
            }

            // Equip primary weapon
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                inventory.EquipWeapon(WeaponType.PRIMARY);
            }

            // Equip secondary weapon
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                inventory.EquipWeapon(WeaponType.SECONDARY);
            }

            // Equip special weapon
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                inventory.EquipWeapon(WeaponType.SPECIAL);
            }

            // Equip throwable weapon
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                inventory.EquipWeapon(WeaponType.THROWABLE);
            }
        }

        // Regenerate a little bit of health...
        if(!IsDead && Health < 10)
        {
            Health += 1 * Time.deltaTime;
            GameManager.Instance.hud.UpdateHealth((int)Health);
        }
    }


    public override void Damage(Hit hit)
    {
        if (IsDead)
            return;

        base.Damage(hit);

        if(Health <= 0)
        {
            Health = 0;
            Die();
            GameManager.Instance.hud.UpdateHealth((int)Health);
        }
        else
        {
            GameManager.Instance.hud.UpdateHealth((int)Health);
        }
    }


    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        GameManager.Instance.hud.UpdateHealth((int)Health);
    }


    public void EnableMovementAndFreeLook(bool value)
    {
        camera3D.enabled = value;
        characterActor.enabled = value;
    }


    public void Die()
    {
        IsDead = true;

        StartCoroutine(DeathCameraEffect());

        Debug.Log("Player is dead...!");
    }


    IEnumerator DeathCameraEffect()
    {
        camera3D.enabled = false;
        characterActor.enabled = false;

        float time = 0;
        Vector3 startPosition = cameraObject.transform.position;
        Vector3 targetPosition = startPosition;

        RaycastHit hit;
        if(Physics.Raycast(cameraObject.transform.position, Vector3.down, out hit, 10f, floorMask))
        {
            targetPosition = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);
        }

        while (time < _deathAnimDuration)
        {
            cameraObject.transform.position = Vector3.Lerp(startPosition, targetPosition, time / _deathAnimDuration);
            time += Time.deltaTime;
            yield return null;
        }

        cameraObject.transform.position = targetPosition;

        // Fade to black after falling to floor...
        LeanTween.value(_colorAdjustments.postExposure.value, -10f, 5f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) => { _colorAdjustments.postExposure.value = val; });
    }


    private void FixedUpdate()
    {
        if (IsDead)
            return;

        // Keep the body directly below the head - for damage and sight checks
        body.position = new Vector3(head.position.x, head.position.y - 0.5f, head.position.z);
    }
}
