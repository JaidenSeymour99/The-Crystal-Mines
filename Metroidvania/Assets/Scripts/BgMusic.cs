using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusic : MonoBehaviour
{
    [SerializeField] float bgMusic;

    void Awake()
    {
        SetBgMusic();
    }


    void SetBgMusic()
    {
        
        AudioManager.instance.StopBgMusic();
        
        if (bgMusic == 1f)
        {
            AudioManager.instance.Lvl1BgMusic();
        }
        else if (bgMusic == 2f)
        {
            AudioManager.instance.Lvl2BgMusic();
        }
        else if(bgMusic == 3f)
        {
            AudioManager.instance.BossMusic();
        }
        


    }



}
