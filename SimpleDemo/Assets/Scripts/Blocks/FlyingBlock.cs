using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBlock : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float flyingSpeed;

    [SerializeField] private float holdStillHeight;

    public bool holdStill;
    
    private void Update()
    {
        if (holdStill) return;
        
       transform.position = Vector2.MoveTowards(transform.position, player.transform.position, flyingSpeed * Time.deltaTime);

       if (transform.position.y < holdStillHeight)
           holdStill = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerCore.instance.PlayerDie();
        }
    }
}
    
