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
    [SerializeField]
    private float immuneDuration = 1f;
    [SerializeField]
    private float immuneTimer = 0f;
    public Vector3 playerMoveDirection;

    public float playerMaxHealth = 100;
    public float playerCurrentHealth;
    private bool isImmune = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        UIController.instance.UpdateHealthSlider();
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

        if (immuneTimer > 0)
        {
            immuneTimer -= Time.deltaTime;
            if (immuneTimer <= 0)
            {
                isImmune = false;
            }
        }
        else
        {
            isImmune = false;
        }

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(playerMoveDirection.x * speed, playerMoveDirection.y * speed);
    }

    public void TakeDamage(float damage)
    {
        if (isImmune)
        {
            return;
        }
        isImmune = true;
        immuneTimer = immuneDuration;
        playerCurrentHealth -= damage;
        UIController.instance.UpdateHealthSlider();
        if (playerCurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        GameManager.instance.GameOver();
    }
}
