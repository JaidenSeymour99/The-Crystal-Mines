using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public BoxCollider2D boxCol; 
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private string[] dialogue;
    private int index;

    [SerializeField] private float wordSpeed;
    public bool playerIsClose;

    private PlayerControls playerControls;

    void Awake()
    {
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

    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        //dialogueText = GetComponent<Text>();
        dialoguePanel = GetComponent<GameObject>();

        dialogueText.text = "";

        playerControls.Player.Interact.performed += _ => TryInteract();
    }

    void Update()
    {
        
    }

    public void TryInteract()
    {
        if (dialoguePanel.activeInHierarchy)
        {
            resetText();
        }
        else if (dialogueText.text == dialogue[index])
        {
            dialoguePanel.SetActive(true);
            StartCoroutine(Typing());
        }
    }

    public void resetText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    public IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if(index < dialogue.Length -1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else 
        {
            resetText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsClose = false;
            resetText();
        }
    }
}
