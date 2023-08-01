using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] AudioSource SFXSource;
    [SerializeField] List<AudioClip> clips = new List<AudioClip>();
    [SerializeField] AudioMixer mixer;


    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSound();
    }


    public void SFX()
    {
        
    }

    public void BackgroundMusic()
    {

    }

    //volume saved in settingsmenu.cs
    void LoadSound()
    {
        float masterVol = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(SettingsMenu.MASTER_MIXER, Mathf.Log10(masterVol) * 20);
        mixer.SetFloat(SettingsMenu.MUSIC_MIXER, Mathf.Log10(musicVol) * 20);
        mixer.SetFloat(SettingsMenu.SFX_MIXER, Mathf.Log10(sfxVol) * 20);
    }

}
