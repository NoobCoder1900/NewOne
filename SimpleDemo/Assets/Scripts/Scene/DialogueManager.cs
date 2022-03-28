using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject catDialogueBox; // display or not
    [SerializeField] private GameObject playerDialogueBox; // display or not

    [SerializeField] private Text catDialogueText;
    [SerializeField] private Text playerDialogueText;
    
    

    [TextArea(1, 3)] 
    [SerializeField] public string[] dialogueLines;
    [SerializeField] private int currentLine;

    private void Awake()
    {
        instance = this;
    }



    private void Update()
    {
        if (playerDialogueBox.activeInHierarchy ||
            catDialogueBox.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (currentLine < dialogueLines.Length)
                {
                    CheckSpeaker();
                }
                else
                {
                    playerDialogueBox.SetActive(false);
                    catDialogueBox.SetActive(false);
                    FindObjectOfType<StartLevelPlayerController>().canMove = true;
                }
            }
        }
    }

    public void ShowDialogueContents(string[] chatLines)
    { 
        if (playerDialogueBox.activeInHierarchy || 
            catDialogueBox.activeInHierarchy) 
            return;
        //如果对话框已经存在，对话已经开始，才会传入对话内容并开启对话
        
        
        dialogueLines = chatLines;
        currentLine = 0;
        
        CheckSpeaker();
        
        FindObjectOfType<StartLevelPlayerController>().canMove = false;
    }

    private void CheckSpeaker()
    {
        if (dialogueLines[currentLine].StartsWith("---"))
        {
            playerDialogueText.text = dialogueLines[currentLine].Replace("---", "");
            currentLine++;
            playerDialogueBox.SetActive(true);
            catDialogueBox.SetActive(false);
        }
        else
        {
            catDialogueText.text = dialogueLines[currentLine];
            currentLine++;
            catDialogueBox.SetActive(true);
            playerDialogueBox.SetActive(false);
        }
    }

}
