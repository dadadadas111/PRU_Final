using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HPUpgradeButton : MonoBehaviour
{
    public TMP_Text description;
    public int amountToIncrease;

    public void RandomizeDescription()
    {
        amountToIncrease = Random.Range(2, 5) * 5;
        description.text = "Max HP +" + amountToIncrease;
    }

    public void Upgrade()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.selectUpgrade);
        PlayerController.instance.IncreaseMaxHealth(amountToIncrease);
        UIController.instance.HideLevelUpPanel();
    }
}
