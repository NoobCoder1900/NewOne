using System;
using UnityEngine;

public class PatrolBlock : MonoBehaviour
{
    // 这个脚本可以让挂在的砖块在两点之间来回移动
    // 注意：这个脚本仅实现移动的功能，砖块本身必须依赖 Block.cs
    
    [SerializeField] private Transform patrolPos1, patrolPos2;
    private Transform patrolTarget;

    [SerializeField] private float moveSpeed;

    private void Start()
    {
        patrolTarget = patrolPos1;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPos1.position) <= 0.01f)
        {
            patrolTarget = patrolPos2;
        }
        if (Vector2.Distance(transform.position, patrolPos2.position) <= 0.01f)
        {
            patrolTarget = patrolPos1;
        }
    }
}
