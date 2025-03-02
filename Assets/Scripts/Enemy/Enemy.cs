using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private GameObject destroyEffect;

    private Vector3 direction;
    private EnemyPool enemyPool;
    private int enemyIndex;

    public void Initialize(EnemyPool pool, int index)
    {
        enemyPool = pool;
        enemyIndex = index;
    }

    void FixedUpdate()
    {
        if (PlayerController.instance != null && PlayerController.instance.gameObject.activeSelf)
        {
            var playerPosition = PlayerController.instance.transform.position;

            // facing the player
            sr.flipX = playerPosition.x > transform.position.x;

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
            PlayerController.instance.TakeDamage(1);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        rb.velocity = Vector2.zero;
        enemyPool.AddToPool(enemyIndex, gameObject);
    }
}