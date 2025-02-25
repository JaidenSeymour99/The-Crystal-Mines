using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LoadNextArea : MonoBehaviour
{

    public static LoadNextArea next;

    private GameObject player;
    private GameObject startPos;

    [SerializeField] bool goNextLevel;
    [SerializeField] string levelName;

    [SerializeField]private GameObject scene1;
    [SerializeField]private GameObject scene2;
    [SerializeField]private GameObject scene3;
    [SerializeField]private GameObject scene4;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        startPos = GameObject.FindGameObjectWithTag("StartPos");
        scene1 = GameObject.FindGameObjectWithTag("Scene1");
        scene2 = GameObject.FindGameObjectWithTag("Scene2");
        scene3 = GameObject.FindGameObjectWithTag("Scene3");
        scene4 = GameObject.FindGameObjectWithTag("Scene4");
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        startPos = GameObject.FindGameObjectWithTag("StartPos");

        player.transform.position = startPos.transform.position;

        scene1 = GameObject.FindGameObjectWithTag("Scene1");
        scene2 = GameObject.FindGameObjectWithTag("Scene2");
        scene3 = GameObject.FindGameObjectWithTag("Scene3");
        scene4 = GameObject.FindGameObjectWithTag("Scene4");
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            UseCam1();
        }

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            UseCam2();
        }
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            UseCam3();
            
        }
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            UseCam4();            
        }
    }

    
    public void UseCam1()
    {
        scene1.SetActive(true);
        scene2.SetActive(false);
        scene3.SetActive(false);
        scene4.SetActive(false);

    }

    void UseCam2()
    {
        scene1.SetActive(false);
        scene2.SetActive(true);
        scene3.SetActive(false);
        scene4.SetActive(false);
    }

    void UseCam3()
    {
        scene1.SetActive(false);
        scene2.SetActive(false);
        scene3.SetActive(true);
        scene4.SetActive(false);
    }
    void UseCam4()
    {
        scene1.SetActive(false);
        scene2.SetActive(false);
        scene3.SetActive(false);
        scene4.SetActive(true);
    }
    // void SwitchPriority(CinemachineVirtualCamera camera)
    // {
    //     camera.Priority = 11;

    // }

    // void SwitchPriorityNormal(CinemachineVirtualCamera camera)
    // {
    //     camera.Priority = 10;
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(goNextLevel)
            {
                SceneController.instance.NextLevel();
            }
            else
            {
                SceneController.instance.LoadScene(levelName);
            }
        }
    }
}
