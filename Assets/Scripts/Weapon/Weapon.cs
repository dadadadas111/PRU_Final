using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public int weaponLevel = 1;
    public int maxLevel = 1;
    public List<WeaponStats> stats;
    public Sprite weaponImage;
    public TMP_Text weaponLevelText;

    public void Start()
    {
        if (weaponLevel < 1)
            weaponLevel = 1;
        maxLevel = stats.Count;
    }

    public void UpgradeWeapon()
    {
        if (weaponLevel < stats.Count)
        {
            weaponLevel++;
        }
    }

    public void UpdateLevel()
    {
        weaponLevelText.text = "Lv." + weaponLevel;
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
