using System;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        TryGetComponent(out boxCollider2D);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boxCollider2D.enabled = !boxCollider2D.enabled;
        }
    }
}
