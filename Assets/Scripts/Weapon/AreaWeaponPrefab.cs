using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponPrefab : MonoBehaviour
{

    public AreaWeapon areaWeapon;
    private Vector3 targetSize;
    private float timer;
    void Start()
    {
        areaWeapon = FindObjectOfType<AreaWeapon>();
        targetSize = Vector3.one * areaWeapon.range;
        transform.localScale = targetSize;
        timer = areaWeapon.duration;
    }

    // Update is called once per frame
    void Update()
    {
        // grow and shrink the object
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 5);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            targetSize = Vector3.zero;
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit");
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(areaWeapon.damage);
            }
        }
    }
}
