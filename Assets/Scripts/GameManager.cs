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
