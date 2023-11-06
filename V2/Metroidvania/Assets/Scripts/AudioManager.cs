using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;


    [SerializeField] AudioSource MusicSource;
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

    public void AttackSFX()
    {
        AudioClip clip = clips[1];
        SFXSource.PlayOneShot(clip);
    }

    public void JumpSFX()
    {
        AudioClip clip = clips[0];
        SFXSource.PlayOneShot(clip);
    }

    public void LandSFX()
    {
        AudioClip clip = clips[2];
        SFXSource.PlayOneShot(clip);
    }

    public void WalkSFX()
    {
        AudioClip clip = clips[3];
        SFXSource.clip = clip;
        SFXSource.loop = true;
        SFXSource.Play();
    }

    public void StopWalkSFX()
    {
        AudioClip clip = clips[3];
        SFXSource.clip = clip;
        SFXSource.Stop();
    }

    public void DashSFX()
    {
        AudioClip clip = clips[4];
        SFXSource.PlayOneShot(clip);
    }


    public void Lvl1BgMusic()
    {
        AudioClip clip = clips[5];
        MusicSource.clip = clip;
        MusicSource.loop = true;
        MusicSource.Play();
    }
    public void Lvl2BgMusic()
    {
        AudioClip clip = clips[6];
        MusicSource.clip = clip;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void BossMusic()
    {
        AudioClip clip = clips[7];
        MusicSource.clip = clip;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void StopBgMusic()
    {
        AudioClip clip1 = clips[5];
        AudioClip clip2 = clips[6];
        AudioClip clip3 = clips[7];
        MusicSource.clip = clip1;
        MusicSource.Stop();
        MusicSource.clip = clip2;
        MusicSource.Stop();
        MusicSource.clip = clip3;
        MusicSource.Stop();
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
