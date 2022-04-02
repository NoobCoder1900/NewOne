using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnim : MonoBehaviour
{
    
    private IPlayerController player;
    private Animator animator;
    private Material currentMaterial;

    private string currentAnim;
    private const string animIdle = "Idle";
    private const string animRun = "Run";
    private const string animJumpUp = "JumpUp";
    private const string animOnlyJumpUp = "OnlyJumpUp";
    private const string animOnlyJumpIdle = "OnlyJumpIdle";
    private const string animOnlyJumpFall = "OnlyJumpFall";
    private const string animArrowDraw = "ArrowDraw";
    private const string animArrowRecoil = "ArrowRecoil";


    [SerializeField] private Texture texIdle;
    [SerializeField] private Texture texRun;
    [SerializeField] private Texture texJumpUp;
    [SerializeField] private Texture texOnlyJumpUp;
    [SerializeField] private Texture texOnlyJumpIdle;
    [SerializeField] private Texture texOnlyJumpFall;
    [SerializeField] private Texture texArrowDraw;
    [SerializeField] private Texture texArrowRecoil;
    private static readonly int NormalMap = Shader.PropertyToID("_NormalMap");
    

    
    private void Awake()
    {
        player = GetComponentInParent<IPlayerController>();
    }
    private void Start()
    {
        currentMaterial = GetComponentInChildren<SpriteRenderer>().material;
        currentMaterial.EnableKeyword("_NormalMap");
        animator = GetComponentInChildren<Animator>();
    }
    

    private void Update()
    {
        if (player == null) return;
        
        Animation();

        MapAnimAndMaterial();

    }
    

    private void Animation()
    {
        
        // 拉弓
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animIdle) && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(true);
            ChangeAnimState(animArrowDraw);
        }
        
        // Run
        if (player.Grounded && player.input.runInput != 0 && player.Velocity.y == 0)
        {
            ChangeAnimState(animRun);
        }
        
        // 原地起跳
        if (player.Grounded && player.input.jumpKeyDown && player.input.runInput == 0)
        {
            ChangeAnimState(animOnlyJumpUp);
        }
        // 助跑起跳
        if (player.Grounded && player.input.jumpKeyDown && player.input.runInput != 0) 
        {
            ChangeAnimState(animJumpUp);
        }

        if (player.Grounded && Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) && player.Velocity.y == 0)
        {
            ChangeAnimState(animIdle);
        }
        
        // 滞空
        if (!player.Grounded && player.Velocity.y < 0)
        {
            ChangeAnimState(animOnlyJumpIdle);
        }

        // 落地
        if (player.Velocity.y < 0 && player.Grounded)
        {
            ChangeAnimState(animOnlyJumpFall);
        }
        
    }
    
    
    
    private void ChangeAnimState(string nextAnim)
    {
        if (currentAnim == nextAnim) return;
        
        animator.Play(nextAnim);

        currentAnim = nextAnim;
    }


    /// <summary>
    /// 根据当前的动画，更换对应的法线贴图
    /// </summary>
    private void MapAnimAndMaterial()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animIdle))
            ChangeMaterialOfAnim(texIdle);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animRun))
            ChangeMaterialOfAnim(texRun);
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animJumpUp))
            ChangeMaterialOfAnim(texJumpUp);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animOnlyJumpUp))
            ChangeMaterialOfAnim(texOnlyJumpUp);
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animOnlyJumpIdle))
            ChangeMaterialOfAnim(texOnlyJumpIdle);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animOnlyJumpFall))
            ChangeMaterialOfAnim(texOnlyJumpFall);
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animArrowDraw))
            ChangeMaterialOfAnim(texArrowDraw);
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animArrowRecoil))
            ChangeMaterialOfAnim(texArrowRecoil);
    }
    
    
    private void ChangeMaterialOfAnim(Texture nextNormal)
    {
        currentMaterial.SetTexture(NormalMap, nextNormal);
    }

    
    
    
}
