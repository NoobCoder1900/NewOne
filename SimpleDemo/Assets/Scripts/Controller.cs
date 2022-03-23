using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour, IPlayerController 
{
    public Vector3 Velocity { get; private set; }
    public FrameInput input { get; private set; }
    public bool JumpingThisFrame { get; private set; }
    public bool LandingThisFrame { get; private set; }
    public Vector3 RawMovement { get; private set; }
    public bool Grounded { get; private set; }

    private Vector3 lastPosition;
    private float currentHorizontalSpeed, currentVerticalSpeed;

    //在执行Update()，存在player的collider组件暂时还未启动的情况
    //为了排除这个问题，执行一下三行代码
    private bool active;
    private void Awake() => Invoke(nameof(Activate), 0.5f);
    private void Activate() => active = true;

    private void Update()
    {
        if (!active) return;
        
        //获取玩家的速度与位置（初始速度为0）
        var position = transform.position;
        lastPosition = position;
        Velocity = (position - lastPosition) / Time.deltaTime;
        
        
        GatherInput();
        RunCollisionChecks();
        CalculateRun();
        CalculateJumpApex();
        CalculateGravity();
        CalculateJump();
        MovePlayer();
    }

    #region Gather Input

    private void GatherInput()
    {
        input = new FrameInput
        {
            jumpKeyDown = Input.GetKeyDown(KeyCode.W),
            jumpKeyUp = Input.GetKeyUp(KeyCode.W),
            runInput = Input.GetAxisRaw("Horizontal")
        };
        if (input.jumpKeyDown)
            lastJumpPressed = Time.time;
    }

    #endregion

    #region Collisions

    [Header("Collisions")] 
    [SerializeField] private Bounds playerBounds;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int detectorCount = 3;
    [SerializeField] private float detectionRayLength = 0.1f;
    [SerializeField] [Range(0f, 0.3f)] private float rayBuffer = 0.08f;//如果没有rayBuffer，side ray 有可能会与脚下的平台碰撞，从而影响操作

    private RayRange rayUp, rayDown, rayLeft, rayRight;
    private bool colUp, colLeft, colRight;

    private float timeLeftGrounded;

    private void RunCollisionChecks()
    {
        CalculateRayRanged();

        LandingThisFrame = false;
        
        //check player是否在踩在platform上
        var groundedCheck = RunDetection(rayDown);
        
        switch (Grounded)
        {
            case true when !groundedCheck:
                timeLeftGrounded = Time.time;
                break;
            case false when groundedCheck:
                coyoteUsable = true;
                LandingThisFrame = true;
                break;
        }

        Grounded = groundedCheck;

        colUp = RunDetection(rayUp);
        colLeft = RunDetection(rayLeft);
        colRight = RunDetection(rayRight);
        
        bool RunDetection(RayRange rayRange)
        {
            return EvaluateRayPositions(rayRange).Any(point => Physics2D.Raycast(point, rayRange.direction, detectionRayLength, groundLayer));
        }
    }

    /// <summary>
    /// 计算根据Player Bound 计算射线范围
    /// </summary>
    private void CalculateRayRanged()
    {
        var bound = new Bounds(transform.position + playerBounds.center, playerBounds.size);

        rayDown = new RayRange(bound.min.x + rayBuffer, bound.min.y, bound.max.x - rayBuffer, bound.min.y, Vector2.down
        );
        rayUp = new RayRange(bound.min.x + rayBuffer, bound.max.y, bound.max.x - rayBuffer, bound.max.y, Vector2.up);
        rayLeft = new RayRange(bound.min.x, bound.min.y + rayBuffer, bound.min.x, bound.max.y - rayBuffer, Vector2.left);
        rayRight = new RayRange(bound.max.x, bound.min.y + rayBuffer, bound.max.x, bound.max.y - rayBuffer, Vector2.right);
        
    }

    /// <summary>
    /// 在Player Bound的四个边框上，各生成detectorCount数量的射线
    /// </summary>
    private IEnumerable<Vector2> EvaluateRayPositions(RayRange rayRange)
    {
        for (var i = 0; i < detectorCount; i++)
        {
            var t = (float) i / (detectorCount - 1);
            yield return Vector2.Lerp(rayRange.start, rayRange.end, t);
        }
    }
    
    /// <summary>
    /// 分别画出Player的Bound、射线、将要移动的位置
    /// </summary>
    private void OnDrawGizmos() {
        // Bounds
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + playerBounds.center, playerBounds.size);

        // Rays
        if (!Application.isPlaying) {
            CalculateRayRanged();
            Gizmos.color = Color.blue;
            foreach (var range in new List<RayRange> { rayUp, rayRight, rayDown, rayLeft }) {
                foreach (var point in EvaluateRayPositions(range)) {
                    Gizmos.DrawRay(point, range.direction * detectionRayLength);
                }
            }
        }

        if (!Application.isPlaying) return;

        // Draw the future position. Handy for visualizing gravity
        Gizmos.color = Color.red;
        var move = new Vector3(currentHorizontalSpeed, currentVerticalSpeed) * Time.deltaTime;
        Gizmos.DrawWireCube(transform.position + move, playerBounds.size);
    }
    
    #endregion

    #region Run

    [Header("Run")] 
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float moveClamp;
    [SerializeField] private float apexBonus;

    private void CalculateRun()
    {
        if (input.runInput != 0) //
        {
            //set horizontal move speed
            currentHorizontalSpeed += input.runInput * acceleration * Time.deltaTime;
            
            currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -moveClamp, moveClamp);
            
            //apply bonus at the apex of a jump
            var bonus = Mathf.Sign(input.runInput) * apexBonus * apexPoint; //apexPoint在跳跃最高点时取值为1
            currentHorizontalSpeed += bonus * Time.deltaTime;
        }
        else
        {
            //没有输入，为player减速
            currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, deceleration * Time.deltaTime);
        }
        
        //当Player在水平方向上有Platform，则速度降为0
        if (currentHorizontalSpeed > 0 && colRight || currentHorizontalSpeed < 0 && colLeft)
        {
            currentHorizontalSpeed = 0;
        }
    }
    #endregion

    #region Gravity

    [SerializeField] private float fallClamp;
    [SerializeField] private float minFallSpeed;
    [SerializeField] private float maxFallSpeed;
    private float fallSpeed;

    private void CalculateGravity()
    {
        if (Grounded)
        {
            //move out of the ground
            if (currentVerticalSpeed < 0) currentVerticalSpeed = 0;
        }
        else
        {
            //在向上跳的过程中，如果松开W键，就向下施加一个力
            
            //判断下降的速度是多少
            var fall = endJumpEarly && currentVerticalSpeed > 0 ? fallSpeed * jumpEndEarlyGravityModifier : fallSpeed;

            currentVerticalSpeed -= fallSpeed * Time.deltaTime;

            if (currentVerticalSpeed < fallClamp) currentVerticalSpeed = fallClamp;
        }
        
        
    }
    
    #endregion

    #region Jump

    [Header("Jump")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpApexThreshold;
    [SerializeField] private float coyoteTimeThreshold;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private float jumpEndEarlyGravityModifier;
    private bool coyoteUsable;
    private bool endJumpEarly;
    private float apexPoint; // become 1 at apex of jump
    private float lastJumpPressed;
    private bool CanUseCoyote => coyoteUsable && !Grounded && timeLeftGrounded + coyoteTimeThreshold > Time.time;
    private bool HasBufferedJump => Grounded && lastJumpPressed + jumpBuffer > Time.time;

    private void CalculateJump()
    {
        //jump if : grounded or within coyote threshold || sufficient jump buffer
        if (input.jumpKeyDown && CanUseCoyote || HasBufferedJump)
        {
            currentVerticalSpeed = jumpHeight;
            endJumpEarly = false;
            coyoteUsable = false;
            timeLeftGrounded = float.MinValue;
            JumpingThisFrame = true;
        }
        else
        {
            JumpingThisFrame = false;
        }
        
        //End jump early if key is released
        if (!Grounded && input.jumpKeyUp && !endJumpEarly && Velocity.y > 0)
        {
            endJumpEarly = true;
        }

        if (colUp)
        {
            if (currentVerticalSpeed > 0)
                currentVerticalSpeed = 0;
        }
    }

    private void CalculateJumpApex()
    {
        if (!Grounded)
        {
            apexPoint = Mathf.InverseLerp(jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
            fallSpeed = Mathf.Lerp(minFallSpeed, maxFallSpeed, apexPoint);
        }
        else
        {
            apexPoint = 0;
        }
    }
    

    #endregion


    #region Move

    [Header("Move")] 
    //此值与碰撞精准度成正比，但会影响性能
    [SerializeField] private int freeColliderIterations = 10;

    private void MovePlayer()
    {
        var pos = transform.position;
        RawMovement = new Vector3(currentHorizontalSpeed, currentVerticalSpeed);
        var move = RawMovement * Time.deltaTime;
        var furthestPoint = move + pos;
        
        //检查将要执行的移动，如果不会发生碰撞就移动，且不执行额外的碰撞检测
        var hit = Physics2D.OverlapBox(furthestPoint, playerBounds.size, groundLayer);
        if (!hit)
        {
            transform.position += move;
            return;
        }

        var positionToMoveTo = transform.position;
        for (int i = 1; i < freeColliderIterations; i++)
        {
            var t = (float) i / freeColliderIterations;
            var posToTry = Vector2.Lerp(pos, furthestPoint, t);

            if (Physics2D.OverlapBox(posToTry, playerBounds.size, groundLayer))
            {
                transform.position = positionToMoveTo;

                if (i == 1)
                {
                    if (currentVerticalSpeed < 0) currentVerticalSpeed = 0;
                    var dir = transform.position - hit.transform.position;
                    transform.position += dir.normalized * move.magnitude;
                }
                
                return;
            }

            positionToMoveTo = posToTry;
        }
        
    }
    
    #endregion
}
