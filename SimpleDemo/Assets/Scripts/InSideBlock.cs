using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InSideBlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCore.instance.insideBlock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCore.instance.insideBlock = false;
        }
    }
}
