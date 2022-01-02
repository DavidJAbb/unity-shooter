/* Weapon data scriptable object */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Weapon Data", menuName="Data/Weapon Data")]
public class WeaponData : ScriptableObject {
    // Weapon data
    public string weaponName;
    public WeaponType weaponType;
    [Tooltip("Lower is more accurate. Higher is less accurate. 0 is perfect accuracy.")]
    public float accuracy;
    [Tooltip("Amount of kickback from weapon - crosshair size increase. 1 is none.")]
    public float kickBack;
    public float damagePerShot;
    public float rateOfFire;
    public int burstCount;
    public float reloadTime;
    public float weaponShakeAmount;
    public float weaponShakeDuration;
    [Tooltip("Single for pistols, Burst for shotguns, Auto for machine guns.")]
    public PlayerGun.FireMode fireMode;
    public AmmoType ammoType;
    public int clipSize;
    public Vector3 aimDownSightsPosition;
}
