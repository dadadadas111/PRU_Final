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
    private bool isSoundPlaying = false;
    void Start()
    {
        areaWeapon = FindObjectOfType<AreaWeapon>();
        targetSize = Vector3.one * areaWeapon.stats[areaWeapon.weaponLevel-1].range;
        transform.localScale = Vector3.zero;
        timer = areaWeapon.stats[areaWeapon.weaponLevel-1].duration;
        AudioManager.instance.PlaySound(AudioManager.instance.areaWeaponSpawn);
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
            // AudioManager.instance.PlaySound(AudioManager.instance.areaWeaponDespawn);
            // above line is commented out because it causes an error. the sound keeps replayed because its in update function
            // fix for this:
            if (isSoundPlaying == false)
            {
                isSoundPlaying = true;
                AudioManager.instance.PlaySound(AudioManager.instance.areaWeaponDespawn);
            }
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }
        // prediodic damage
        counter -= Time.deltaTime * areaWeapon.stats[areaWeapon.weaponLevel-1].speed;
        if (counter <= 0)
        {
            counter = 1;
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].TakeDamage(areaWeapon.stats[areaWeapon.weaponLevel-1].damage);
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
