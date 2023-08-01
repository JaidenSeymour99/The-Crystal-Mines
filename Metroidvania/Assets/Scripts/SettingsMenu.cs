using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public AudioMixer audioMixer;
    
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;


    public const string MASTER_MIXER = "mastervolume";
    public const string MUSIC_MIXER = "musicvolume";
    public const string SFX_MIXER = "sfxvolume";

    void Awake()
    {

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);


    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }
    
    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {

        audioMixer.SetFloat(MASTER_MIXER, Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {   
        audioMixer.SetFloat(MUSIC_MIXER, Mathf.Log10(volume) * 20);
    }
    
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(SFX_MIXER, Mathf.Log10(volume) * 20);
    }


    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }


}
