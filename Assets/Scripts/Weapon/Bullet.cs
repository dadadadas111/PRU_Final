using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private Enemy target;

    public void Initialize(float bulletDamage, float bulletSpeed, Enemy enemyTarget)
    {
        damage = bulletDamage;
        speed = bulletSpeed;
        target = enemyTarget;
    }

    void Update()
    {
        if (!target || !target.gameObject.activeInHierarchy)
        {
            BulletPool.Instance.ReturnBullet(this);
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.gameObject == target.gameObject)
        {
            target.TakeDamageNoPush(damage);
            BulletPool.Instance.ReturnBullet(this);
        }
    }
}
