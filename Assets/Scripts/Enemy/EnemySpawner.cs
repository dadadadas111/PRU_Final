using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawnTimer;
        public float spawnInterval = 2f;
        public int enemiesPerWave = 10;
        public int spawnedEnemies = 0;
    }

    public List<Wave> waves;
    public int waveNumber = 0;
    public Transform minPos;
    public Transform maxPos;

    void Update()
    {
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
                    SpawnEnemy();
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
                    waveNumber++;
                }
            }
        }
        else
        {
            waveNumber = 0;
        }
    }

    void SpawnEnemy()
    {
        Instantiate(waves[waveNumber].enemyPrefab, RandomPosition(), Quaternion.identity);
    }

    Vector2 RandomPosition()
    {
        Vector2 spawnPoint;

        if (Random.Range(0f, 1f) > 0.5f)
        {
            spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.y = minPos.position.y;
            }
            else
            {
                spawnPoint.y = maxPos.position.y;
            }
        }
        else
        {
            spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.x = minPos.position.x;
            }
            else
            {
                spawnPoint.x = maxPos.position.x;
            }
        }

        return spawnPoint;
    }
}
