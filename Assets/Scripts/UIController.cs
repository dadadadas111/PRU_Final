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
    public UpgradeButton[] upgradeButtons;
    [SerializeField]
    private TMP_Text timerText;

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

    public void UpdateTimer(float timer)
    {
        float min = Mathf.FloorToInt(timer / 60);
        float sec = Mathf.FloorToInt(timer % 60);
        timerText.text = min + ":" + sec.ToString("00");
    }

    public void ShowLevelUpPanel()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideLevelUpPanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
