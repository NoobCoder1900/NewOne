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

    private readonly Vector3 left = new Vector3(-1, 1, 1);
    private readonly Vector3 right = new Vector3(1, 1, 1);

    private void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out animator);
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        
        
        if (moveInput != 0)
            transform.localScale = (moveInput > 0) ? right : left;
        
        if (rb.velocity != Vector2.zero)
            animator.Play("PlayerWalkSL");
        else 
            animator.Play("PlayerIdleSL");
        
        
    }

    private void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPow) * Mathf.Sign(speedDiff);
        
        rb.AddForce(movement * Vector2.right);

    }
}
