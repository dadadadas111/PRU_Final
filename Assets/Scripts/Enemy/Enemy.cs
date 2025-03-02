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

    void FixedUpdate()
    {
        if (PlayerController.instance != null && PlayerController.instance.gameObject.activeSelf)
        {
            var playerPosition = PlayerController.instance.transform.position;

            // facing the player
            if (playerPosition.x > transform.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
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
            PlayerController.instance.TakeDamage(1);
            Destroy(gameObject);
            Instantiate(destroyEffect, transform.position, transform.rotation);
        }
    }
}
