using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider sfxSlider;

    private bool isToggleMusic, isToggleSFX;
    private Button musicButton, sfxButton;

    void Start()
    {
        musicButton = GetComponent<Button>();
        sfxButton = GetComponent<Button>();

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.3f);;
    }

    public void ToggleMusic()
    {
        isToggleMusic = !isToggleMusic;
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        isToggleSFX = !isToggleSFX;
        AudioManager.instance.ToggleSFX();
    }

    public void SetMusicVolume()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SetSFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
}
