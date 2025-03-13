using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PickupItemPool : MonoBehaviour
{
    public static PickupItemPool Instance;

    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pickupPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject pickup = Instantiate(pickupPrefab, transform);
            pickup.SetActive(false);
            pickupPool.Enqueue(pickup);
        }
    }

    public GameObject GetPickup(Vector3 position)
    {
        GameObject pickup;
        if (pickupPool.Count > 0)
        {
            pickup = pickupPool.Dequeue();
        }
        else
        {
            pickup = Instantiate(pickupPrefab, transform);
        }
        
        pickup.transform.position = position;
        pickup.SetActive(true);
        return pickup;
    }

    public void ReturnPickup(GameObject pickup)
    {
        pickup.SetActive(false);
        pickupPool.Enqueue(pickup);
    }

    public void DropPickups(int exp, Transform transform)
    {
        for (int i = 0; i < exp / 5; i++)
        {
            GameObject pickup = PickupItemPool.Instance.GetPickup(transform.position);
            pickup.GetComponent<PickupItem>().SetType(0);
            pickup.GetComponent<PickupItem>().Initialize(transform.position);
        }
    }

    public void RandomDropItem(Vector3 position, int rate)
    {
        if (Random.Range(0, 100) > rate) return; // Random drop rate
        int type = Random.Range(1, 3); // Random type
        GameObject pickup = PickupItemPool.Instance.GetPickup(position);
        pickup.GetComponent<PickupItem>().SetType(type);
        pickup.GetComponent<PickupItem>().Initialize(position);
    }

    private IEnumerator SlowDownPickup(Rigidbody2D rb)
    {
         yield return new WaitForSeconds(0.2f); // Allow initial movement

        float duration = 0.5f; // How long to slow down
        float elapsed = 0f;
        Vector2 initialVelocity = rb.velocity;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // Progress (0 to 1)
            rb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, t); // Smoothly reduce velocity
            yield return null;
        }

        rb.velocity = Vector2.zero; // Ensure it fully stops
    }
}
