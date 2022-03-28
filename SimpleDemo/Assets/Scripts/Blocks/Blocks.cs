using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{

    private bool isBlackBox;
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        TryGetComponent(out boxCollider2D);

        isBlackBox = GetComponent<SpriteRenderer>().color == Color.black;
    }

    /// <summary>
    /// 这个方法用来控制砖块的碰撞体的开关与否
    /// 在PlayerCore.cs中的Update()调用
    /// </summary>
    public void ChangeState()
    {
        switch (PlayerCore.instance.blackOrNot)
        {
            case false: //白砖块
                boxCollider2D.enabled = !isBlackBox;
                break;
            case true: //黑砖块
                boxCollider2D.enabled = isBlackBox;
                break;
        }
    }
}
