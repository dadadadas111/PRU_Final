using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public int type = 0;
    // 0: Experience, 1: Health, 2: MoveSpeed...
    // sprite list for different types of pickups
    public Sprite[] pickupSprites;

    private Vector2 targetPosition;
    private float moveSpeed = 4f;
    private bool isMoving = false;
    private Transform player;  // Reference to player
    private bool isMagnetActive = false;
    private float magnetRange = 3.5f; // How close before the magnet activates
    private float magnetSpeed = 10f; // Speed when attracted
    private float magnetAcceleration = 5f; // How quickly speed increases
    private float currentMagnetSpeed; // Dynamic speed variable
    private float despawnDistance = 30f;

    public void Initialize(Vector2 dropPosition)
    {
        // Set target position a bit away from drop point
        targetPosition = dropPosition + Random.insideUnitCircle * 1.5f;
        isMoving = true;
        isMagnetActive = false;
        currentMagnetSpeed = magnetSpeed;
    }

    public void SetType(int type)
    {
        this.type = type;
        GetComponent<SpriteRenderer>().sprite = pickupSprites[type];
    }

    private void Start()
    {
        player = PlayerController.instance.transform;
    }

    private void Update()
    {
        if (isMagnetActive)
        {
            // Move toward the player with increasing speed
            // transform.position = Vector2.MoveTowards(transform.position, player.position, magnetSpeed * Time.deltaTime);
            // Gradually increase speed when moving toward player
            currentMagnetSpeed += magnetAcceleration * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, player.position, currentMagnetSpeed * Time.deltaTime);
        }
        else if (isMoving)
        {
            // Normal drop movement
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop when close enough
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                // isMagnetActive = true;
            }
        }

        // Activate magnet effect when within range
        if (Vector2.Distance(transform.position, player.position) < magnetRange && !isMoving)
        {
            // if type is HP but player is full, don't activate magnet
            if (type == 1 && PlayerController.instance.playerCurrentHealth == PlayerController.instance.playerMaxHealth)
            {
                return;
            }

            isMagnetActive = true;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        // **Despawn if too far from player**
        if (distanceToPlayer > despawnDistance)
        {
            Debug.Log("Item Despawned");
            PickupItemPool.Instance.ReturnPickup(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        // Implement pickup effect here (e.g., increase score, add health, etc.)
        switch (type)
        {
            case 0:
                // Experience
                AudioManager.instance.PlayModifiedSound(AudioManager.instance.expGet);
                PlayerController.instance.GetExperience(5);
                break;
            case 1:
                // Health
                AudioManager.instance.PlayModifiedSound(AudioManager.instance.healthGet);
                PlayerController.instance.Heal(20);
                break;
            case 2:
                // MoveSpeed
                AudioManager.instance.PlayModifiedSound(AudioManager.instance.speedBuffGet);
                PlayerController.instance.BuffSpeed(10f, 5f);
                break;
            default:
                break;
        }
        PickupItemPool.Instance.ReturnPickup(gameObject);
    }
}
