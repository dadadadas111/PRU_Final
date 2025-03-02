using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverPanel());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Pause(){

        // if is game over, do not pause
        if (UIController.instance.gameOverPanel.activeSelf)
        {
            return;
        }

        if (UIController.instance.pausePanel.activeSelf)
        {
            UIController.instance.pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            UIController.instance.pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        UIController.instance.gameOverPanel.SetActive(true);
    }

}
