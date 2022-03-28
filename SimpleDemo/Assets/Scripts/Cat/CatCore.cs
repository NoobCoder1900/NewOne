using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCore : MonoBehaviour
{
    [SerializeField] private GameObject indicator;

    [SerializeField] private bool isAbleToTalk;

    public int talkCount = 0;

    private bool catStart;

    [TextArea(1, 2)] public string[] firstChatContents;

    [TextArea(1, 2)] public string[] noMoreChatContents;

    private void Start()
    {
        indicator.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            indicator.SetActive(true);
            isAbleToTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            indicator.SetActive(false);
            isAbleToTalk = false;
        }
    }

    private void Update()
    {
        if (isAbleToTalk && Input.GetKeyUp(KeyCode.UpArrow) && talkCount == 0)
        {
            indicator.SetActive(false); 
            DialogueManager.instance.ShowDialogueContents(firstChatContents);
            talkCount++;
        }

        if (isAbleToTalk && Input.GetKeyUp(KeyCode.UpArrow) && talkCount == 1)
        {
            indicator.SetActive(false);
            DialogueManager.instance.ShowDialogueContents(noMoreChatContents);
        }
        
        CatWalk();
    }

    private void CatWalk()
    {
        catStart = FindObjectOfType<SignOnTheLeft>().sceneStart;
        
        if (!catStart) return;
        
        GetComponent<Animator>().Play("CatWalk");
        GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
    }
    
}
