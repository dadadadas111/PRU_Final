using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : Weapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzlePoint; // The gun's shooting position
    [SerializeField] private float detectionRadius = 6f; // How far it can detect enemies

    private Enemy targetEnemy;
    private float shootCounter;

    void Update()
    {
        FindTarget();
        if (targetEnemy)
        {
            RotateTowardsTarget();
            ShootAtTarget();
        }
    }

    void FindTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestDistance = detectionRadius;
        Enemy closestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (!enemy.gameObject.activeInHierarchy) continue; // Ignore inactive enemies

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (targetEnemy != null && targetEnemy != closestEnemy) 
            targetEnemy.SetTarget(false);

        targetEnemy = closestEnemy;

        if (targetEnemy)
            targetEnemy.SetTarget(true);
    }

    void RotateTowardsTarget()
    {
        if (!targetEnemy) return;
        Vector3 direction = targetEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void ShootAtTarget()
    {
        shootCounter -= Time.deltaTime;
        if (shootCounter <= 0)
        {
            shootCounter = stats[weaponLevel - 1].cooldown;
            if (targetEnemy)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        var bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = muzzlePoint.position;
        bullet.transform.rotation = muzzlePoint.rotation;
        bullet.Initialize(stats[weaponLevel - 1].damage, stats[weaponLevel - 1].speed, targetEnemy);
        AudioManager.instance.PlaySound(AudioManager.instance.gunShoot);
    }

}
