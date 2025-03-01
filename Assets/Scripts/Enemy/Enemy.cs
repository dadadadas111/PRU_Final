using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    void Update()
    {
        if (PlayerController.instance != null)
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
        }
    }
}
