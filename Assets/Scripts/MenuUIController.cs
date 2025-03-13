using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIController : MonoBehaviour
{
    public static MenuUIController instance;

    public GameObject loadButton;

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
        if (PlayerPrefs.HasKey("canLoadData"))
        {
            if (PlayerPrefs.GetInt("canLoadData") == 1)
            {
                loadButton.SetActive(true);
            }
            else
            {
                loadButton.SetActive(false);
            }
        }
        else 
        {
            loadButton.SetActive(false);
        }
    }
    
}
