using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    [SerializeField] private GameObject dialogueBox; // display or not
    [SerializeField] private Text dialogueText;

    [TextArea(1, 3)] 
    [SerializeField] public string[] dialogueLines;
    [SerializeField] private int currentLine;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dialogueText.text = dialogueLines[currentLine];
    }

    private void Update()
    {
        if (dialogueBox.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                currentLine++;

                if (currentLine < dialogueLines.Length)
                    dialogueText.text = dialogueLines[currentLine];
                else
                {
                    dialogueBox.SetActive(false);
                    FindObjectOfType<StartLevelPlayerController>().canMove = true;
                }
            }
        }
    }

    public void ShowDialogueContents(string[] chatLines)
    {
        if (dialogueBox.activeInHierarchy) return;
        
        dialogueLines = chatLines;
        currentLine = 0;
        dialogueText.text = dialogueLines[currentLine];
        dialogueBox.SetActive(true);
        FindObjectOfType<StartLevelPlayerController>().canMove = false;
    }
    
}
