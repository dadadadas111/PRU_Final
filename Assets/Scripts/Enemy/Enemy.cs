using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float damage = 1f;
    [SerializeField]
    private float health = 5f;
    [SerializeField]
    private int exp = 1;
    [SerializeField]
    private float pushTime = 0.2f;
    public float currentHealth;

    [SerializeField]
    private GameObject destroyEffect;

    [SerializeField]
    private GameObject targetIndicator;

    private Vector3 direction;
    private float pushCounter;
    private EnemyPool enemyPool;
    private int enemyIndex;

    private float despawnDistance = 30f;

    public void Initialize(EnemyPool pool, int index)
    {
        enemyPool = pool;
        enemyIndex = index;
    }

    void OnEnable()
    {
        currentHealth = health;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.instance.transform.position);
        // **Despawn if too far from player**
        if (distanceToPlayer > despawnDistance)
        {
            Debug.Log("Enemy respawned near player");
            EnemySpawner.instance.SpawnEnemy(enemyIndex);
            ReturnToPool();
        }
    }

    void FixedUpdate()
    {
        if (PlayerController.instance != null && PlayerController.instance.gameObject.activeSelf)
        {
            var playerPosition = PlayerController.instance.transform.position;

            // facing the player
            sr.flipX = playerPosition.x > transform.position.x;

            // push back
            if (pushCounter > 0)
            {
                pushCounter -= Time.deltaTime;
                if (speed > 0)
                {
                    speed = -speed;
                }
                if (pushCounter <= 0)
                {
                    speed = Mathf.Abs(speed);
                }
            }

            // moving towards the player
            direction = (playerPosition - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().isUntouchable)
            {
                return;
            }
            PlayerController.instance.TakeDamage(damage);
            currentHealth = 0;
        }
    }

    void ReturnToPool()
    {
        SetTarget(false);
        rb.velocity = Vector2.zero;
        // reset health
        currentHealth = health;
        enemyPool.AddToPool(enemyIndex, gameObject);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        pushCounter = pushTime;
        DamageNumberController.instance.CreateNumber(transform.position, (int)damage);
        if (currentHealth <= 0)
        {
            Die();
        }
        AudioManager.instance.PlayModifiedSound(AudioManager.instance.enemyHit);
    }

    public void TakeDamageNoPush(float damage)
    {
        currentHealth -= damage;
        DamageNumberController.instance.CreateNumber(transform.position, (int)damage);
        if (currentHealth <= 0)
        {
            Die();
        }
        AudioManager.instance.PlayModifiedSound(AudioManager.instance.enemyHit);
    }

    public void IncreateStats(int loopCount)
    {
        currentHealth += loopCount * 2;
        damage += loopCount;
        speed += loopCount * 0.2f;
    }

    void Die()
    {
        AudioManager.instance.PlayModifiedSound(AudioManager.instance.enemyDeath);
        Instantiate(destroyEffect, transform.position, transform.rotation, enemyPool.transform);
        // if player not reach max level
        if (PlayerController.instance.currentLevel < PlayerController.instance.maxLevel)
        {
            PickupItemPool.Instance.DropPickups(exp, transform);
            PickupItemPool.Instance.RandomDropItem(transform.position, 30);
        }
        ReturnToPool();
    }

    // Method to toggle target indicator
    public void SetTarget(bool isTargeted)
    {
        if (targetIndicator != null)
        {
            targetIndicator.SetActive(isTargeted);
        }
    }
}