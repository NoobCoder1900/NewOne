using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour
{
    #region Component References

    private IPlayerController player;
    [SerializeField] private Animator animator;
    private Material currentMaterial;

    #endregion
    
    #region AnimationName
    
    private string currentAnim;

    private const string PlayerIdle = "Idle";
    private const string PlayerRun = "Run";
    private const string PlayerJumpingUp = "Jumping Up";
    private const string PlayerFalling = "Falling";
    private const string PlayerLanding = "Landing";
    
    
    #endregion

    #region Materials of animations

    
    [SerializeField] private Texture idle;
    [SerializeField] private Texture run;
    [SerializeField] private Texture jump;
    [SerializeField] private Texture fall;
    [SerializeField] private Texture land;

    #endregion

    
    private void Awake()
    {
        player = GetComponentInParent<IPlayerController>();
    }


    private void Start()
    {
        currentMaterial = GetComponentInChildren<SpriteRenderer>().material;
        currentMaterial.EnableKeyword("_NormalMap");
    }

    private void Update()
    {
        if (player == null) return;
        
        SetMaterialAndAnimation();
    }


    private void SetMaterialAndAnimation()
    {
        if (player.Grounded  && player.Velocity.y == 0)
        {
            if (player.input.runInput != 0)
            {
                ChangeMaterialOfAnim(run);

                ChangeAnimState(PlayerRun);
            }

            if (player.Velocity.x != 0 && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)))
            {
                ChangeMaterialOfAnim(idle);

                ChangeAnimState(PlayerIdle);
            }
               
        }
        
        if (player.Grounded && player.input.jumpKeyDown)
        {
            ChangeMaterialOfAnim(jump);

            ChangeAnimState(PlayerJumpingUp);
        }

        if (!player.Grounded && player.Velocity.y < 0)
        {
            ChangeMaterialOfAnim(fall);

            ChangeAnimState(PlayerFalling);
        }

        if (player.Grounded && player.Velocity.y < 0)
        {
            ChangeMaterialOfAnim(land);

            ChangeAnimState(PlayerLanding);
        }
        
        Debug.Log(currentMaterial.GetTexture("_NormalMap").name);
    }
    
    private void ChangeAnimState(string nextAnim)
    {
        if (currentAnim == nextAnim) return;
        
        animator.Play(nextAnim);

        currentAnim = nextAnim;
    }

    private void ChangeMaterialOfAnim(Texture nextNormal)
    {
        if (currentMaterial.GetTexture("_NormalMap").name == nextNormal.name) 
            return;
        else 
            currentMaterial.SetTexture("_NormalMap", nextNormal);
    }
    
    
}
