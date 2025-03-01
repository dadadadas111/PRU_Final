using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float speed = 2f;
    public Vector3 playerMoveDirection;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        playerMoveDirection = new Vector3(inputX, inputY).normalized;

        anim.SetFloat("moveX", inputX);
        anim.SetFloat("moveY", inputX != 0 ? 0 : inputY);

        if (playerMoveDirection == Vector3.zero)
        {
            anim.SetBool("moving", false);
        }
        else
        {
            anim.SetBool("moving", true);
        }

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(playerMoveDirection.x * speed, playerMoveDirection.y * speed);
    }
}
