using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List of enemy prefabs
    public int initialPoolSize = 5;
    
    private List<Queue<GameObject>> enemyPools = new List<Queue<GameObject>>();

    // variable to see the total numbers of enemies in the pool
    public int totalActiveEnemies = 0;
    public int totalEnemies = 0;

    private void Start()
    {
        // Initialize a queue for each enemy prefab
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyPools.Add(new Queue<GameObject>());
            GrowPool(i, initialPoolSize);
        }
    }

    public void GrowPool(int index, int amount)
    {
        if (index < 0 || index >= enemyPrefabs.Count) return;
        
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefabs[index]);
            enemy.transform.SetParent(transform);
            enemy.SetActive(false);
            totalEnemies++;
            enemyPools[index].Enqueue(enemy);
        }
    }

    public GameObject GetFromPool(int index)
    {
        if (index < 0 || index >= enemyPools.Count) return null;
        
        if (enemyPools[index].Count == 0)
        {
            GrowPool(index, 3); // Expand if empty
        }

        GameObject enemy = enemyPools[index].Dequeue();
        enemy.SetActive(true);
        totalActiveEnemies++;
        enemy.GetComponent<Enemy>().Initialize(this, index);
        return enemy;
    }

    public void AddToPool(int index, GameObject enemy)
    {
        if (index < 0 || index >= enemyPools.Count) return;
        
        enemy.SetActive(false);
        totalActiveEnemies--;
        enemyPools[index].Enqueue(enemy);
    }
}
