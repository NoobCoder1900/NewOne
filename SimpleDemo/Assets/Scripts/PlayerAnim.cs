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
        

        if (player.Grounded && player.input.runInput != 0 && player.Velocity.y == 0)
        {
            ChangeAnimState(PlayerRun);
           
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
