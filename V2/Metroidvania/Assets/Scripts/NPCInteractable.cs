using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteractable : MonoBehaviour
{

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject interact;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] dialogue;
    private int index = 0;
    private bool playerIsClose;

    [SerializeField] private GameObject contButton;
    [SerializeField] private float wordSpeed;
    

    // void Start()
    // {
    //     interact = GameObject.FindGameObjectWithTag("Interact");
    // }

    // void Update()
    // {
    //     showInteractIcon();
    // }

    //no time to make this not bug out when skipping through the text too fast.
    public void Interact()
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            resetText();
            dialoguePanel.SetActive(true);
            StartCoroutine(Typing());
        }
        else if (dialoguePanel.activeInHierarchy)
        {
            NextLine();
        }
        else if (dialogueText.text == dialogue[index])
        {
            NextLine();
        }
        else 
        {
            resetText();
        }

        
    }

    public void resetText()
    {
        StopCoroutine(Typing());
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
            StopCoroutine(Typing());
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else 
        {
            resetText();
        }
    }

    // void showInteractIcon()
    // {
    //     if(playerIsClose)
    //     {
    //         interact.SetActive(true);
    //     }
    //     else 
    //     {
    //         interact.SetActive(false);
    //     }
    // }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsClose = true;
            resetText();
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
