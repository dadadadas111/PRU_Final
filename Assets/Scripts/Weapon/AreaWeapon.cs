using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeapon : Weapon
{
    [SerializeField]
    private GameObject prefab;
    private float spawnCounter;

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = stats[weaponLevel-1].cooldown;
            var weapon = Instantiate(prefab, transform.position, Quaternion.identity);
            weapon.transform.SetParent(transform);
        }
    }
}
