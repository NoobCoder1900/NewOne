using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvertColor : MonoBehaviour
{
    private SpriteRenderer sp;
    private float changeRequest = 0.25f;
    private static readonly int Threshold = Shader.PropertyToID("_Threshold");

    private void Start()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            changeRequest = 1 - changeRequest;
            sp.material.SetFloat(Threshold, changeRequest);
        }
    }
}
