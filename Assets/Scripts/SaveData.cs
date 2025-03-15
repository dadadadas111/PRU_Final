using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // player data and game time to save
    public float playerMaxHealth;
    public float playerCurrentHealth;
    public int experience;
    public int currentLevel;
    public float[] playerPosition;
    public float gameTime;
    public int[] weaponLevels;

    // enemies current wave
    public int waveNumber;
    public int loopCount;

    // enemies data to save
    // TODO: save the enemies position and type
    public int[] activeEnemiesIndex;
    public float[][] activeEnemiesPosition;


    public SaveData(PlayerController player, GameManager gameManager, EnemySpawner enemySpawner, EnemyPool enemyPool)
    {
        // player
        playerMaxHealth = player.playerMaxHealth;
        playerCurrentHealth = player.playerCurrentHealth;
        experience = player.experience;
        currentLevel = player.currentLevel;
        playerPosition = new float[3];
        weaponLevels = new int[2];
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;
        gameTime = gameManager.gameTime;
        var weapons = player.activeWeapons;
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponLevels[i] = weapons[i].weaponLevel;
        }

        // enemy
        waveNumber = enemySpawner.waveNumber;
        loopCount = enemySpawner.loopCount;

        // enemies active data
        var activeEnemiesData = enemyPool.GetActiveEnemyData();
        activeEnemiesIndex = new int[activeEnemiesData.Count];
        activeEnemiesPosition = new float[activeEnemiesData.Count][];
        for (int i = 0; i < activeEnemiesData.Count; i++)
        {
            activeEnemiesIndex[i] = activeEnemiesData[i].enemyIndex;
            activeEnemiesPosition[i] = activeEnemiesData[i].position;
        }
    }

    public void PrintSaveData()
    {
        // Debug.Log("Player Max Health: " + playerMaxHealth);
        // Debug.Log("Player Current Health: " + playerCurrentHealth);
        // Debug.Log("Experience: " + experience);
        // Debug.Log("Current Level: " + currentLevel);
        // Debug.Log("Player Position: " + playerPosition[0] + ", " + playerPosition[1] + ", " + playerPosition[2]);
        // Debug.Log("Game Time: " + gameTime);
        // for (int i = 0; i < weaponLevels.Length; i++)
        // {
        //     Debug.Log("Weapon " + i + " Level: " + weaponLevels[i]);
        // }
    }
}
