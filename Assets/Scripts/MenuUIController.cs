using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIController : MonoBehaviour
{
    public static MenuUIController instance;

    [SerializeField] 
    private List<TMP_Text> scoreTexts; 

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

    public void ShowHighScorePanel()
    {
        // load the scene for high score
        SceneManager.LoadScene("HighScore");
    }
    
}
