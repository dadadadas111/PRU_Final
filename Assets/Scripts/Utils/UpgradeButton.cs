using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public TMP_Text weaponName;
    public TMP_Text weaponDescription;
    public TMP_Text weaponLevel;
    public Image weaponImage;

    private Weapon assignedWeapon;

    public void ActivateButton(Weapon weapon)
    {
        var currentLevel = weapon.weaponLevel;
        if (currentLevel > 0 ){
            weaponName.text = weapon.name + " + " + currentLevel;
            weaponDescription.text = weapon.stats[weapon.weaponLevel - 1].description;
        }
        else{
            weaponName.text = weapon.name;
            weaponDescription.text = "Unlock weapon";
        }
        weaponImage.sprite = weapon.weaponImage;
        assignedWeapon = weapon;
        Time.timeScale = 0;
    }

    public void UpgradeWeapon()
    {
        if (assignedWeapon.weaponLevel >= assignedWeapon.maxLevel) return;
        AudioManager.instance.PlaySound(AudioManager.instance.selectUpgrade);
        if (assignedWeapon.weaponLevel == 0)
        {
            // set active weapon
            assignedWeapon.weaponLevel = 1;
            assignedWeapon.gameObject.SetActive(true);
            // start game
            GameManager.instance.isGameStarted = true;
        }
        else 
        {
            assignedWeapon.UpgradeWeapon();
        }
        weaponLevel.text = "Lv." + assignedWeapon.weaponLevel;
        UIController.instance.HideLevelUpPanel();
        UIController.instance.startWeaponPanel.SetActive(false);
    }
}
