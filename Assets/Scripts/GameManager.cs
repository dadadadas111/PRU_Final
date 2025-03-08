using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float gameTime;
    private bool isGameOver = false;

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
    }

    void Update()
    {
        CheatKeys();
        if (isGameOver)
        {
            return;
        }
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
    }

    public void GameOver()
    {
        isGameOver = true;
        StartCoroutine(ShowGameOverPanel());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Pause()
    {

        // if is game over, do not pause
        if (UIController.instance.gameOverPanel.activeSelf)
        {
            return;
        }

        if (UIController.instance.pausePanel.activeSelf)
        {
            UIController.instance.pausePanel.SetActive(false);
            Time.timeScale = 1;
            AudioManager.instance.PlaySound(AudioManager.instance.unpause);
        }
        else
        {
            UIController.instance.pausePanel.SetActive(true);
            Time.timeScale = 0;
            AudioManager.instance.PlaySound(AudioManager.instance.pause);
        }
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

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlaySound(AudioManager.instance.playerDeath);
        UIController.instance.gameOverPanel.SetActive(true);
    }

}
