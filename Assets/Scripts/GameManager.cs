using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float gameTime;
    public bool isGameOver = false;
    public bool isGameStarted = false;
    public bool isGameContinueFromSave = false;
    // for each game, only 3 times save is allowed
    public int saveLimit = 3;

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
        isGameOver = false;
        var gameMode = PlayerPrefs.GetInt("gameMode", 0);
        if (gameMode == 0) // normal mode
        {
            isGameStarted = false;
            if (UIController.instance != null)
            {
                PlayerPrefs.SetInt("currentSaveLeft", saveLimit);
                // PlayerPrefs.SetInt("canLoadData", 0);
                UIController.instance.saveNoticeText.text = "SAVES LEFT: " + saveLimit;
            }
        }
        else // continue mode
        {
            LoadGame();
            isGameOver = false;
            isGameStarted = true;
            Time.timeScale = 1;
            if (UIController.instance != null)
            {
                var currentSaveLeft = PlayerPrefs.GetInt("currentSaveLeft", saveLimit);
                UIController.instance.startWeaponPanel.SetActive(false);
                UIController.instance.saveNoticeText.text = "SAVES LEFT: " + currentSaveLeft;
            }
        }
    }

    void Update()
    {
        if (isGameOver || !isGameStarted)
        {
            return;
        }
        CheatKeys();
        gameTime += Time.deltaTime;
        if (UIController.instance == null)
        {
            return;
        }
        UIController.instance.UpdateTimer(gameTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void CheatKeys()
    {
        // num 1 to level up
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerController.instance.GetExperience(PlayerController.instance.playerLevels[PlayerController.instance.currentLevel - 1]);
        }

        // num 2 to heal
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerController.instance.Heal(100);
        }

        // key t to take damage
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerController.instance.TakeDamage(5);
        }

        // num 3 to disable/enable weapon
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerController.instance.ToggleWeapon();
        }

        // num 4 to be untouchable
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerController.instance.TogglePlayerCollision();
        }

        // num 5 to fast forward / slow down time
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 2;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        // // key i to save game, key o to load game
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     SaveGame();
        // }
        // if (Input.GetKeyDown(KeyCode.O))
        // {
        //     LoadGame();
        // }
    }

    public void GameOver()
    {
        isGameOver = true;
        PlayerPrefs.SetInt("canLoadData", 0);
        HighScoreManager.Instance.SaveScore(gameTime);
        UIController.instance.UpdateResultText();
        StartCoroutine(ShowGameOverPanel());
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("canLoadData", 0);
        PlayerPrefs.SetInt("gameMode", 0);
        SceneManager.LoadScene("Game");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("gameMode", 1);
        var currentSaveLeft = PlayerPrefs.GetInt("currentSaveLeft", saveLimit);
        if (currentSaveLeft <= 0)
        {
            PlayerPrefs.SetInt("canLoadData", 0);
            return;
        }
        currentSaveLeft--;
        PlayerPrefs.SetInt("currentSaveLeft", currentSaveLeft);
        if (currentSaveLeft <= 0)
        {
            PlayerPrefs.SetInt("canLoadData", 0);
        }
        // ex: (You can save the game 2 more times./n Use wisely)
        SceneManager.LoadScene("Game");
    }

    public void Pause()
    {
        // if is game over or is leveling up, do not pause
        if (UIController.instance.gameOverPanel.activeSelf || UIController.instance.levelUpPanel.activeSelf)
        {
            return;
        }

        if (UIController.instance.pausePanel.activeSelf)
        {
            UIController.instance.pausePanel.SetActive(false);
            // if is leveling up, do not unpause
            if (!UIController.instance.levelUpPanel.activeSelf)
            {
                Time.timeScale = 1;
            }
            AudioManager.instance.PlaySound(AudioManager.instance.unpause);
        }
        else
        {
            UIController.instance.pausePanel.SetActive(true);
            Time.timeScale = 0;
            AudioManager.instance.PlaySound(AudioManager.instance.pause);
        }
        SaveGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(PlayerController.instance, GameManager.instance, EnemySpawner.instance, EnemyPool.instance);
        PlayerPrefs.SetInt("canLoadData", 1);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null)
        {
            return;
        }
        // log the data first
        data.PrintSaveData();
        if (PlayerController.instance == null)
        {
            return;
        }
        PlayerController.instance.LoadPlayerData(
            data.playerMaxHealth,
            data.playerCurrentHealth,
            data.experience,
            data.currentLevel,
            data.playerPosition,
            data.weaponLevels
        );
        if (EnemySpawner.instance == null)
        {
            return;
        }
        EnemySpawner.instance.LoadEnemyData(data.waveNumber, data.loopCount);
        if (EnemyPool.instance == null)
        {
            return;
        }
        EnemyPool.instance.LoadEnemyData(data.activeEnemiesIndex, data.activeEnemiesPosition);
        LoadGameManager(data);
        isGameContinueFromSave = true;
    }

    public void LoadGameManager(SaveData data)
    {
        gameTime = data.gameTime;
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlaySound(AudioManager.instance.playerDeath);
        UIController.instance.gameOverPanel.SetActive(true);
    }

}
