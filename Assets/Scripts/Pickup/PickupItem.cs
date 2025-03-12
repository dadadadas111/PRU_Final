using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Vector2 targetPosition;
    private float moveSpeed = 4f;
    private bool isMoving = false;
    private Transform player;  // Reference to player
    private bool isMagnetActive = false;
    private float magnetRange = 3.5f; // How close before the magnet activates
    private float magnetSpeed = 10f; // Speed when attracted

    public void Initialize(Vector2 dropPosition)
    {
        // Set target position a bit away from drop point
        targetPosition = dropPosition + Random.insideUnitCircle * 1.5f;
        isMoving = true;
        isMagnetActive = false;
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
            transform.position = Vector2.MoveTowards(transform.position, player.position, magnetSpeed * Time.deltaTime);
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
            isMagnetActive = true;
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
        PlayerController.instance.GetExperience(5);
        AudioManager.instance.PlayModifiedSound(AudioManager.instance.expGet);
        PickupItemPool.Instance.ReturnPickup(gameObject);
    }
}
