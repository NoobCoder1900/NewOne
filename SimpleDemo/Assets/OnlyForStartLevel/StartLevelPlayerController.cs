using UnityEngine;

public class StartLevelPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private float moveInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    private const float velocityPow = 0.9f;

    public bool canMove = true; 

    private bool active;
    private void Awake() => Invoke(nameof(Activate), 6f);
    private void Activate() => active = true;

    private bool inputToStart;
    
    private void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out animator);
    }

    private void Update()
    {
        if (!active) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        Flip();
        
        SetAnim();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        
        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPow) * Mathf.Sign(speedDiff);
        
        rb.AddForce(movement * Vector2.right);

    }

    private void Flip()
    {
        if (!canMove) return;
        
        if (moveInput != 0)
            transform.localScale = (moveInput > 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

    }

    private void SetAnim()
    {
        if (rb.velocity != Vector2.zero)
        {
            animator.Play("Walk");
            inputToStart = true;
        }
        if (inputToStart && rb.velocity == Vector2.zero)
            animator.Play("Idle");
    }
}
