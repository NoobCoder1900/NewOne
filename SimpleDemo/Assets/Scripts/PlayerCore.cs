using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    #region Component References
    
    public static PlayerCore instance;
    private SpriteRenderer sp;
    [SerializeField] private Transform playerRevivePos;
    
    #endregion
    
    // Invert Color
    private float changeRequest = 0.25f;
    private static readonly int Threshold = Shader.PropertyToID("_Threshold");
    
    // Fall to death height
    [SerializeField] private float deathHeight = -4.5f;
    

    public bool insideBlock;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != this) 
                Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        InvertColor();

        FallToDeath();
    }

    /// <summary>
    /// When "Space" is pressed, invert the color of material of player 
    /// </summary>
    private void InvertColor()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (insideBlock == false)
            {
                changeRequest = 1 - changeRequest;
                sp.material.SetFloat(Threshold, changeRequest);
            }
            else
            {
                Die();
            }
        }
    }

    private void FallToDeath() 
    {
        if (transform.position.y <= deathHeight)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Debug.Log("YOU DIED");
        sp.enabled = false;
        StartCoroutine(Revive());
    }

    IEnumerator  Revive()
    {
        yield return new WaitForSeconds(1f);
        transform.position = playerRevivePos.position;
        sp.enabled = true;
        GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero;
    }
    
    
}
