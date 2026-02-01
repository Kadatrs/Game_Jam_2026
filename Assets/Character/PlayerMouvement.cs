using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    Vector2 movement;

    float lastMoveX = 0f;
    float lastMoveY = -1f;

    public Vector2 LastMoveDirection => new Vector2(lastMoveX, lastMoveY).normalized;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0 || movement.y != 0)
        {
            if (movement.x > 0) spriteRenderer.flipX = false;
            else if (movement.x < 0) spriteRenderer.flipX = true;

            lastMoveX = movement.x;
            lastMoveY = movement.y;
        }

        animator.SetFloat("MoveX", lastMoveX);
        animator.SetFloat("MoveY", lastMoveY);
        animator.speed = 1;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
