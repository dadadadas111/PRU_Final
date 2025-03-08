using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponLevel = 1;
    public List<WeaponStats> stats;
    public Sprite weaponImage;

    public void UpgradeWeapon()
    {
        if (weaponLevel < stats.Count)
        {
            weaponLevel++;
        }
    }
}

[System.Serializable]
public class WeaponStats
{
    public float cooldown;
    public float duration;
    public float damage;
    public float range;
    public float speed;
    public string description;
}
