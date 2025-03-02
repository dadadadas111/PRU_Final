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

    public void GameOver()
    {
        StartCoroutine(ShowGameOverPanel());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        UIController.instance.gameOverPanel.SetActive(true);
    }

}
