using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField] private Sound[] musicSounds, sfxSounds;
    [SerializeField] private AudioSource musicSource, sfxSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic("MusicTheme");
    }

    public void PlayMusic(string audioName)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == audioName);

        AudioClip audioClip = Resources.Load<AudioClip>(audioName);
        if (audioClip != null || audioClip == sound.audioClip)
        {
            Debug.Log("Audio is already playing");
            return;
        }
        else
        {
            if (sound == null)
            {
                Debug.Log("Sound was not found");
            }
            else
            {
                musicSource.clip = sound.audioClip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }        
    }

    public void PlaySFX(string audioName)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == audioName);

        if (sound == null)
        {
            Debug.Log("Sound was not found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.audioClip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
