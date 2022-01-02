using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

// An inventory to hold all of the players items.
public class PlayerInventory : MonoBehaviour
{
    // Store different ammo types and and amounts of that ammo
    public List<AmmoSlot> ammo = new List<AmmoSlot>();
    public List<WeaponSlot> weapons = new List<WeaponSlot>();
    public List<Key> keys = new List<Key>();

    // Weapon Slots
    // MELEE - crowbar?
    // PRIMARY - Pistol / revolver
    // SECONDARY - shotgun
    // SPECIAL - flare gun
    // THROWABLE - bottles / molotovs

    public GameObject[] weaponPrefabs;
    private GameObject[] _weaponClones;

    [HideInInspector]
    public PlayerWeapon curWeapon;


    private void Awake()
    {
        // Set up empty weapon slots (see above)
        weapons.Add(new WeaponSlot(false, null));
        weapons.Add(new WeaponSlot(false, null));
        weapons.Add(new WeaponSlot(false, null));
        weapons.Add(new WeaponSlot(false, null));
        weapons.Add(new WeaponSlot(false, null));

        _weaponClones = new GameObject[weaponPrefabs.Length];
        for(int i = 0; i < weaponPrefabs.Length; i++)
        {
            GameObject clone = Instantiate(weaponPrefabs[i], GameManager.Instance.player.handsTransform.position, GameManager.Instance.player.handsTransform.rotation, GameManager.Instance.player.handsTransform) as GameObject;
            _weaponClones[i] = clone;
            _weaponClones[i].SetActive(false);
        }
    }


    public void AssignWeaponToSlot(WeaponType type, GameObject go)
    {
        if(weapons[(int)type].weapon == null || weapons[(int)type].weapon != go)
        {
            if (curWeapon != null && curWeapon.data.weaponType == type)
            {
                // Remove the current weapon.
                UnequipWeapon(type);
                weapons[(int)type].weapon = null;
            }

            weapons[(int)type].weapon = go;
            weapons[(int)type].weapon.GetComponent<PlayerWeapon>().Init(GameManager.Instance.player);
        }
    }

    
    public void EquipWeapon(WeaponType weaponType)
    {
        if(weapons[(int)weaponType].weapon == null)
        {
            Debug.Log("No weapon in slot to equip");
            return;
        }

        if (curWeapon != null)
        {
            UnequipWeapon(curWeapon.data.weaponType);
        }

        curWeapon = weapons[(int)weaponType].weapon.GetComponent<PlayerWeapon>();
        curWeapon.Equip();
        weapons[(int)weaponType].isActive = true;
    }


    public void AddWeapon(WeaponType type, int index)
    {
        AssignWeaponToSlot(type, _weaponClones[index]);
        EquipWeapon(type);
    }


    public void UnequipWeapon(WeaponType weaponType)
    {
        weapons[(int)weaponType].weapon.GetComponent<PlayerWeapon>().Unequip();
        weapons[(int)weaponType].isActive = false;
    }


    // AMMO METHODS
    public bool CanPickupAmmoOfType(AmmoType ammoType, int amount)
    {
        // Return true if we have less than the max ammo of type...
        if (ammo[(int)ammoType].curAmount < ammo[(int)ammoType].maxAmount)
        {
            return true;
        }
        return false;
    }

    // Get total ammo by type
    public int GetTotalAmmo(AmmoType ammoType) {
        return ammo[(int)ammoType].curAmount;
    }

    // REMOVE ammo by type - e.g. from gun loading into a clip
    public void RemoveAmmo(AmmoType ammoType, int value) {
    	ammo[(int)ammoType].curAmount -= value;

    	// Check and enforce min value of 0
    	if(ammo[(int)ammoType].curAmount < 0) {
    		ammo[(int)ammoType].curAmount = 0;
    	}
    }

    // ADD ammo by type - e.g. from a pickup or gun returning bullets from clip
    public void AddAmmo(AmmoType ammoType, int value) {
    	ammo[(int)ammoType].curAmount += value;

    	// Check and enforce max ammo of type
    	if(ammo[(int)ammoType].curAmount > ammo[(int)ammoType].maxAmount) {
    		ammo[(int)ammoType].curAmount = ammo[(int)ammoType].maxAmount;
    	}

        // Update the HUD if adding the currently used ammo type
        if(ammoType == curWeapon.data.ammoType)
        {
            curWeapon.UpdateHUD();
        }
    }


    public void AddKey(Key keyToAdd)
    {
        keys.Add(keyToAdd);

        Debug.Log($"Added key:'{keyToAdd.keyName}' to inventory");
    }


    public bool HasKey(int idToMatch)
    {
        for(int i = 0; i < keys.Count; i++)
        {
            if(keys[i].keyID == idToMatch)
            {
                Debug.Log($"Player has '{keys[i].keyName}'");
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class Key
{
    public string keyName;
    public int keyID;

    public Key(string name, int id)
    {
        this.keyName = name;
        this.keyID = id;
    }
}

[System.Serializable]
public class AmmoSlot
{
    public AmmoType ammoType;
    public int curAmount;
    public int maxAmount;

    public AmmoSlot(AmmoType type, int amount, int max)
    {
        this.ammoType = type;
        this.curAmount = amount;
        this.maxAmount = max;
    }
}

[System.Serializable]
public class WeaponSlot
{
    public bool isActive;
    public GameObject weapon;

    public WeaponSlot(bool active, GameObject weapon)
    {
        this.isActive = active;
        this.weapon = weapon;
    }
}

public enum AmmoType
{
    PISTOL,
    REVOLVER,
    SHOTGUN
}

public enum WeaponType
{
    MELEE,
    PRIMARY,
    SECONDARY,
    SPECIAL,
    THROWABLE
}
