using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponPrefab : MonoBehaviour
{

    public AreaWeapon areaWeapon;
    private Vector3 targetSize;
    private float timer;
    private float counter;
    public List<Enemy> enemiesInRange = new List<Enemy>();
    void Start()
    {
        areaWeapon = FindObjectOfType<AreaWeapon>();
        targetSize = Vector3.one * areaWeapon.range;
        transform.localScale = Vector3.zero;
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
        // prediodic damage
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            counter = areaWeapon.damagePeriod;
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].TakeDamage(areaWeapon.damage);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.GetComponent<Enemy>());
        }
    }
}
