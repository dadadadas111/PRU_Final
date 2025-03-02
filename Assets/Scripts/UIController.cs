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
    public GameObject gameOverPanel;

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

    public void UpdateHealthSlider()
    {
        healthSlider.maxValue = PlayerController.instance.playerMaxHealth;
        healthSlider.value = PlayerController.instance.playerCurrentHealth;
        healthText.text = PlayerController.instance.playerCurrentHealth + " / " + PlayerController.instance.playerMaxHealth;
    }
}
