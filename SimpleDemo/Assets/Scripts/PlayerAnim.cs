using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    #region Component References

    private IPlayerController player;
    [SerializeField] private Animator animator;

    #endregion
    
    #region AnimationName
    
    private string currentAnim;

    private const string PlayerIdle = "Idle";
    private const string PlayerRun = "Run";
    private const string PlayerJumpingUp = "Jumping Up";
    private const string PlayerFalling = "Falling";
    private const string PlayerLanding = "Landing";
    
    
    #endregion




    private void Awake()
    {
        player = GetComponentInParent<IPlayerController>();
    }
    
    
    private void Update()
    {
        if (player == null) return;
        
        //唯一的问题，不能解决 Run --> Idle 的转变
        // 没有 input.y

        if (player.Grounded  && player.Velocity.y == 0)
        {
            if (player.input.runInput != 0) 
                ChangeAnimState(PlayerRun);
            if (player.Velocity.x != 0 && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)))
                ChangeAnimState(PlayerIdle);
        }
        
        if (player.Grounded && player.input.jumpKeyDown)
        {
            ChangeAnimState(PlayerJumpingUp);
        }

        if (!player.Grounded && player.Velocity.y < 0)
        {
            ChangeAnimState(PlayerFalling);
        }

        if (player.Grounded && player.Velocity.y < 0)
        {
            ChangeAnimState(PlayerLanding);
        }

    }

    
    
    private void ChangeAnimState(string nextAnim)
    {
        if (currentAnim == nextAnim) return;
        
        animator.Play(nextAnim);

        currentAnim = nextAnim;
    }
    
    
    
}
