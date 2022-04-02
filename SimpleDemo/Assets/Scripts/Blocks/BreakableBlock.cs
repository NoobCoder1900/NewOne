using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    // Breakable砖块：在玩家为踩踏之前是正常的方块
    // 玩家从该砖块上起跳，就会消失

    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Material dissolveMaterial;
    private static readonly int AlphaClipThreshold = Shader.PropertyToID("_AlphaClipThreshold");
    private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");

    // 脚本是挂载在砖块的子物体上的，销毁时需要销毁整个砖块
    [SerializeField] private GameObject ParentBlock;
    
    
    private float dissolveAmount;
    [SerializeField] private float dissolveSpeed;// 控制砖块溶解的速度

    private bool playerLeave; // 只有玩家离开砖块时，才会触发砖块溶解
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerLeave = true;
        }
        
    }

    private void Update()
    {
        if (!playerLeave) return;
        
        DissolveBlock();

    }

    private void DissolveBlock()
    {
        if (dissolveAmount <= 1)
        {
            dissolveAmount += (Time.deltaTime * dissolveSpeed);
        }

        sp.material = dissolveMaterial;
        sp.material.SetColor(EdgeColor, Color.black);
        sp.material.SetFloat(AlphaClipThreshold, dissolveAmount);

        if (Mathf.Abs(dissolveAmount - 1f) <= 0.01f)
        {
            Destroy(ParentBlock);
        } 
    }
}
