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
    private float speed = 5f;
    [SerializeField]
    private float immuneDuration = 1f;
    [SerializeField]
    private float immuneTimer = 0f;
    public Vector3 playerMoveDirection;

    public float playerMaxHealth = 100;
    public float playerCurrentHealth;
    public int experience = 0;
    public int currentLevel = 1;
    public int maxLevel = 10;
    public List<int> playerLevels; 
    private bool isImmune = false;
    public bool isUntouchable = false;

    public Weapon[] activeWeapons;

    private float buffSpeedTimer = 0;

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
        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            var levelExp = (Mathf.CeilToInt(playerLevels[playerLevels.Count-1] * 1.1f + 10));
            // round to the nearest number divislbe by 5
            levelExp = Mathf.CeilToInt(levelExp / 5) * 5;
            playerLevels.Add(levelExp);
        }
        playerCurrentHealth = playerMaxHealth;
        UIController.instance.UpdateHealthSlider();
        UIController.instance.UpdateExpSlider();
        for (int i = 0; i < activeWeapons.Length; i++)
        {
            UIController.instance.startUpgradeButtons[i].ActivateButton(activeWeapons[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
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

        if (buffSpeedTimer > 0)
        {
            buffSpeedTimer -= Time.deltaTime;
            if (buffSpeedTimer <= 0)
            {
                speed = 5f;
            }
        }
        else
        {
            speed = 5f;
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
        if (playerCurrentHealth <= 0)
        {
            playerCurrentHealth = 0;
            Die();
        }
        UIController.instance.UpdateHealthSlider();
    }

    public void Heal(float healAmount)
    {
        playerCurrentHealth += healAmount;
        if (playerCurrentHealth > playerMaxHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }
        UIController.instance.UpdateHealthSlider();
    }

    public void BuffSpeed(float time, float amount)
    {
        speed += amount;
        speed = Mathf.Clamp(speed, 5f, 10f);
        // prevent stacking speed buff
        buffSpeedTimer = time;
    }

    public void IncreaseMaxHealth(float amount)
    {
        playerMaxHealth += amount;
        playerCurrentHealth += amount;
        UIController.instance.UpdateHealthSlider();
    }

    public void Die()
    {
        gameObject.SetActive(false);
        GameManager.instance.GameOver();
    }

    public void GetExperience(int exp)
    {
        if (currentLevel >= maxLevel)
        {
            UIController.instance.UpdateExpSliderMax(); 
            return;
        }
        experience += exp;
        if (experience >= playerLevels[currentLevel - 1] && playerCurrentHealth > 0 && currentLevel < maxLevel)
        {
            LevelUp();
        }
        UIController.instance.UpdateExpSlider(); 
    }

    public void LevelUp() 
    {
        AudioManager.instance.PlaySound(AudioManager.instance.levelUp);
        if (currentLevel >= maxLevel)
        {
            return;
        }
        experience -= playerLevels[currentLevel - 1];
        currentLevel++;
        for (int i = 0; i < activeWeapons.Length; i++)
        {
            UIController.instance.upgradeButtons[i].ActivateButton(activeWeapons[i]);
        }
        UIController.instance.hpUpgradeButton.RandomizeDescription();
        UIController.instance.ShowLevelUpPanel();
    }

    public void ToggleWeapon()
    {
        for (int i = 0; i < activeWeapons.Length; i++)
        {
            activeWeapons[i].gameObject.SetActive(!activeWeapons[i].gameObject.activeSelf);
        }
    }

    public void TogglePlayerCollision()
    {
        isUntouchable = !isUntouchable;   
    }
}
