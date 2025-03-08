using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Menu sounds
    public AudioSource pause;
    public AudioSource unpause;
    public AudioSource hover;
    public AudioSource click;

    // Game sounds
    public AudioSource enemyHit;
    public AudioSource enemyDeath;
    public AudioSource selectUpgrade;
    public AudioSource areaWeaponSpawn;
    public AudioSource areaWeaponDespawn;
    public AudioSource playerDeath;
    public AudioSource levelUp;

    private void Awake()
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

    public void PlaySound(AudioSource audio)
    {
        audio.Stop();
        audio.Play();
    }

    public void PlayModifiedSound(AudioSource audio)
    {
        audio.pitch = Random.Range(0.7f, 1.3f);
        PlaySound(audio);
    }
}
