using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private Slider expSlider;
    [SerializeField]
    private TMP_Text expText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject levelUpPanel;
    public GameObject startWeaponPanel;
    public GameObject highScorePanel;
    public UpgradeButton[] upgradeButtons;
    public UpgradeButton[] startUpgradeButtons;
    public HPUpgradeButton hpUpgradeButton;
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private TMP_Text gameResultText;
    [SerializeField] 
    private List<TMP_Text> scoreTexts; 
    [SerializeField]
    private TMP_Text levelText;

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

    public void UpdateExpSlider()
    {
        expSlider.maxValue = PlayerController.instance.playerLevels[PlayerController.instance.currentLevel - 1];
        expSlider.value = PlayerController.instance.experience;
        expText.text = PlayerController.instance.experience + " / " + PlayerController.instance.playerLevels[PlayerController.instance.currentLevel - 1];
    }

    public void UpdateHealthSlider()
    {
        healthSlider.maxValue = PlayerController.instance.playerMaxHealth;
        healthSlider.value = PlayerController.instance.playerCurrentHealth;
        healthText.text = PlayerController.instance.playerCurrentHealth + " / " + PlayerController.instance.playerMaxHealth;
    }

    public void UpdateExpSliderMax()
    {
        expSlider.value = expSlider.maxValue;
        expText.text = "MAX";
    }

    public void UpdateTimer(float timer)
    {
        float min = Mathf.FloorToInt(timer / 60);
        float sec = Mathf.FloorToInt(timer % 60);
        timerText.text = min + ":" + sec.ToString("00");
    }

    public void UpdateResultText()
    {
        string prefix = "You survived for ";
        string suffix = " !";
        // get the timer text to display the result
        gameResultText.text = prefix + timerText.text + suffix;
    }

    public void ShowLevelUpPanel()
    {
        levelText.text = "Lv." + PlayerController.instance.currentLevel;
        levelUpPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideLevelUpPanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowHighScorePanel()
    {
        highScorePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        ReloadHighScore();
    }

    public void ReloadHighScore()
    {
        List<string> scores = HighScoreManager.Instance.GetFormatedHighScores();
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            string prefix = (i + 1) + ". ";
            if (i < scores.Count)
            {
                scoreTexts[i].text = prefix + scores[i];
            }
            else
            {
                scoreTexts[i].text = prefix + "X:XX";
            }
        }
    }

    public void HideHighScorePanel()
    {
        highScorePanel.SetActive(false);
        if (GameManager.instance.isGameOver)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
