using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLoad : MonoBehaviour
{
    public static DestroyOnLoad instance;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            
        }
        else 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
