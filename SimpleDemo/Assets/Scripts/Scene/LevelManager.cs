using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D rightBoundary;

    [SerializeField] private GameObject exit;

    [SerializeField] private Image greyBackground;

    private bool readyToStart;

    [SerializeField] private float fillSpeed;
    
    private void Update()
    {
        readyToStart = FindObjectOfType<SignOnTheLeft>().sceneStart;
        
        if (readyToStart)
        {
            rightBoundary.enabled = false;
            greyBackground.fillAmount += Time.deltaTime * fillSpeed;
            exit.SetActive(true);
        }
    }
}
