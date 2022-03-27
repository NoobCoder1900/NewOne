using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIndicator : MonoBehaviour
{
    [SerializeField] private GameObject indicator;

    [SerializeField] private bool isAbleToTalk;

    [TextArea(1, 2)] 
    public string[] catChatContents;

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
        if (isAbleToTalk && Input.GetKeyUp(KeyCode.UpArrow))
        {
            indicator.SetActive(false); 
            DialogueManager.instance.ShowDialogueContents(catChatContents);
        }
    }
}
