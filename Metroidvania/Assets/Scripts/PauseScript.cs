using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] public static bool isPaused = false;
    [SerializeField] public static bool isDead = false;
    private PlayerControls playerControls;

    


    void Awake()
    {
        isPaused = false;
        isDead = false;
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            healthBarUI.SetActive(false);
            isPaused = false;
            isDead = false;
            mainMenuUI.SetActive(true);
        } else 
        {

            mainMenuUI.SetActive(false);
            healthBarUI.SetActive(true);

        }


        playerControls.Menu.Escape.performed += _ => PauseCheck();
    }

    void Update()
    {
        if(isDead)
        {
            StartCoroutine(PauseGameDeath());
        }
        else StopCoroutine(PauseGameDeath());

        if(SceneManager.GetActiveScene().buildIndex >= 1)
        {
            mainMenuUI.SetActive(false);
        }
        
    }

    private void PauseCheck()
    {
        if(isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        
        isPaused = true;
        pauseUI.SetActive(true);
        healthBarUI.SetActive(true);
    }
    public IEnumerator PauseGameDeath()
    {
        yield return new WaitForSeconds(.8f);
        Time.timeScale = 0;
        isDead = true;
        isPaused = true;
        deathUI.SetActive(true);
        yield return null;
    }

    public void RestartGame()
    {

        isPaused = false;
        isDead = false;
        deathUI.SetActive(false);
        healthBarUI.SetActive(true);
        Time.timeScale = 1;
        SceneController.instance.RestartScene();
        
    }


    public void ResumeGame()
    {
        Time.timeScale = 1;
        
        isPaused = false;
        pauseUI.SetActive(false);
        healthBarUI.SetActive(true);
    }
}
