using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerMaxHealth;
    public float playerCurrentHealth;
    public int experience;
    public int currentLevel;
    public float[] playerPosition;
    public float gameTime;

    public SaveData(PlayerController player, GameManager gameManager)
    {
        playerMaxHealth = player.playerMaxHealth;
        playerCurrentHealth = player.playerCurrentHealth;
        experience = player.experience;
        currentLevel = player.currentLevel;
        playerPosition = new float[3];
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;
        gameTime = gameManager.gameTime;
    }

    public void PrintSaveData()
    {
        Debug.Log("Player Max Health: " + playerMaxHealth);
        Debug.Log("Player Current Health: " + playerCurrentHealth);
        Debug.Log("Experience: " + experience);
        Debug.Log("Current Level: " + currentLevel);
        Debug.Log("Player Position: " + playerPosition[0] + ", " + playerPosition[1] + ", " + playerPosition[2]);
        Debug.Log("Game Time: " + gameTime);
    }
}
