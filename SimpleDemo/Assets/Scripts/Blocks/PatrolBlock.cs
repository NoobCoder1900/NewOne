using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBlock : MonoBehaviour
{
    // 这个脚本可以让挂在的砖块在两点之间来回移动
    // 注意：这个脚本仅实现移动的功能，砖块本身必须依赖 Block.cs

    [SerializeField] private Transform patrolPoint1, patrolPoint2;
    private Transform patrolTarget;

    [SerializeField] private float patrolSpeed;

    private void Start()
    {
        patrolTarget = patrolPoint1;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, patrolTarget.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPoint1.position) < 0.01f)
        {
            patrolTarget = patrolPoint2;
        }
        
        if (Vector2.Distance(transform.position, patrolPoint2.position) < 0.01f)
        {
            patrolTarget = patrolPoint1;
        }
    }
}
