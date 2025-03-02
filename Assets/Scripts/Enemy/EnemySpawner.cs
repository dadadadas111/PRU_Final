using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyIndex; // Index for EnemyPool
        public float spawnTimer;
        public float spawnInterval = 2f;
        public int enemiesPerWave = 10;
        public int nextWaveMultiplier = 2;
        public int maxEnemiesPerWave = 100;
        public int spawnedEnemies = 0;
    }

    public List<Wave> waves;
    public int waveNumber = 0;
    public Transform minPos;
    public Transform maxPos;
    public EnemyPool enemyPool; // Reference to EnemyPool

    void Update()
    {
        if (!PlayerController.instance.gameObject.activeSelf)
        {
            return;
        }
        if (waveNumber < waves.Count)
        {
            Wave wave = waves[waveNumber];
            wave.spawnTimer += Time.deltaTime;
            if (wave.spawnTimer >= wave.spawnInterval)
            {
                wave.spawnTimer = 0;
                if (wave.spawnedEnemies < wave.enemiesPerWave)
                {
                    wave.spawnedEnemies++;
                    SpawnEnemy(wave.enemyIndex);
                }
                else
                {
                    wave.spawnedEnemies = 0;
                    wave.spawnInterval *= 0.9f;
                    // set min spawn interval
                    if (wave.spawnInterval < 0.3f)
                    {
                        wave.spawnInterval = 0.3f;
                    }
                    wave.enemiesPerWave *= wave.nextWaveMultiplier;
                    if (wave.enemiesPerWave > wave.maxEnemiesPerWave)
                    {
                        wave.enemiesPerWave = wave.maxEnemiesPerWave;
                    }
                    waveNumber++;
                }
            }
        }
        else
        {
            waveNumber = 0;
        }
    }

    void SpawnEnemy(int enemyIndex)
    {
        GameObject enemy = enemyPool.GetFromPool(enemyIndex);
        if (enemy != null)
        {
            enemy.transform.position = RandomPosition();
            enemy.transform.rotation = Quaternion.identity;
        }
    }

    Vector2 RandomPosition()
    {
        Vector2 spawnPoint;

        if (Random.Range(0f, 1f) > 0.5f)
        {
            spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
            spawnPoint.y = Random.Range(0f, 1f) > 0.5f ? minPos.position.y : maxPos.position.y;
        }
        else
        {
            spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
            spawnPoint.x = Random.Range(0f, 1f) > 0.5f ? minPos.position.x : maxPos.position.x;
        }

        return spawnPoint;
    }
}
