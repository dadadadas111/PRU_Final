using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private float spawnCounter;
    public float cooldown = 5f;
    public float duration = 3f;
    public float damage = 3f;
    public float range = 1f;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = cooldown;
            var weapon = Instantiate(prefab, transform.position, Quaternion.identity);
            weapon.transform.SetParent(transform);
        }
    }
}
