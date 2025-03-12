using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer audioMixer;

    private bool isBGMMuted = false;
    private bool isSFXMuted = false;

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
    public AudioSource gunShoot;
    public AudioSource expGet;
    public AudioSource healthGet;
    public AudioSource speedBuffGet;

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

    public void ToggleBGM()
    {
        isBGMMuted = !isBGMMuted;
        audioMixer.SetFloat("BGM", isBGMMuted ? -80f : -10f);
    }

    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;
        audioMixer.SetFloat("SFX", isSFXMuted ? -80f : 0f);
    }
}
