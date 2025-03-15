using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;

    public List<GameObject> enemyPrefabs; // List of enemy prefabs
    public int initialPoolSize = 5;
    
    private List<Queue<GameObject>> enemyPools = new List<Queue<GameObject>>();

    // variable to see the total numbers of enemies in the pool
    public int totalActiveEnemies = 0;
    public int totalEnemies = 0;

    public void Awake()
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

    [System.Serializable]
    public class ActiveEnemyData
    {
        // this class aim to define data needed to save the enemy position and type
        public int enemyIndex;
        public float[] position;
    }

    public List<ActiveEnemyData> GetActiveEnemyData()
    {
        List<ActiveEnemyData> activeEnemyData = new List<ActiveEnemyData>();
        for (int i = 0; i < enemyPools.Count; i++)
        {
            foreach (var enemy in enemyPools[i])
            {
                // if the enemy is not active, skip it
                if (!enemy.activeSelf) continue;
                ActiveEnemyData data = new ActiveEnemyData();
                data.enemyIndex = i;
                data.position = new float[3];
                data.position[0] = enemy.transform.position.x;
                data.position[1] = enemy.transform.position.y;
                data.position[2] = enemy.transform.position.z;
                activeEnemyData.Add(data);
            }
        }
        return activeEnemyData;
    }

    public void LoadEnemyData(int[] activeEnemiesIndex, float[][] activeEnemiesPosition)
    {
        Start();
        // Debug.Log("Enemy queue count: " + enemyPools.Count);
        for (int i = 0; i < activeEnemiesIndex.Length; i++)
        {
            // Debug.Log("Load enemy data: " + activeEnemiesIndex[i] + " " + activeEnemiesPosition[i][0] + " " + activeEnemiesPosition[i][1] + " " + activeEnemiesPosition[i][2]);
            GameObject enemy = GetFromPool(activeEnemiesIndex[i]);
            enemy.transform.position = new Vector3(activeEnemiesPosition[i][0], activeEnemiesPosition[i][1], activeEnemiesPosition[i][2]);
        }
    }
}
