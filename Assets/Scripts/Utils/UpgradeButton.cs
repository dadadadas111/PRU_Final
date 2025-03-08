using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public TMP_Text weaponName;
    public TMP_Text weaponDescription;
    public Image weaponImage;

    private Weapon assignedWeapon;

    public void ActivateButton(Weapon weapon)
    {
        var currentLevel = weapon.weaponLevel;
        weaponName.text = weapon.name + " + " + currentLevel;
        weaponDescription.text = weapon.stats[weapon.weaponLevel - 1].description;
        weaponImage.sprite = weapon.weaponImage;
        assignedWeapon = weapon;
    }

    public void UpgradeWeapon()
    {
        if (assignedWeapon.weaponLevel >= assignedWeapon.maxLevel) return;
        AudioManager.instance.PlaySound(AudioManager.instance.selectUpgrade);
        assignedWeapon.UpgradeWeapon();
        UIController.instance.HideLevelUpPanel();
    }
}
