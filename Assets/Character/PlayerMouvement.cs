using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    Vector2 movement;
    // Khởi tạo hướng mặc định là nhìn xuống (0, -1)
    float lastMoveX = 0f;
    float lastMoveY = -1f;

    void Update()
    {
        // 1. Lấy input từ phím
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. Chỉ cập nhật hướng cuối cùng KHI người chơi có bấm phím
        if (movement.x != 0 || movement.y != 0)
        {
            // Xử lý Lật hình (Flip)
            if (movement.x > 0) spriteRenderer.flipX = false;
            else if (movement.x < 0) spriteRenderer.flipX = true;

            // Ghi nhớ hướng
            lastMoveX = movement.x;
            lastMoveY = movement.y;

            // Mẹo: Nếu đi chéo, ta có thể ưu tiên một hướng để tránh lỗi Blend Tree
            // Nếu bạn muốn ưu tiên hướng ngang khi đi chéo:
            // if (movement.x != 0) lastMoveY = 0; 
        }

        // 3. TRUYỀN GIÁ TRỊ VÀO ANIMATOR
        // Thay vì truyền 'movement', ta truyền 'lastMove' 
        // Điều này giúp Blend Tree luôn giữ đúng vị trí của hướng cuối cùng
        animator.SetFloat("MoveX", lastMoveX);
        animator.SetFloat("MoveY", lastMoveY);

        // 4. LUÔN LUÔN PHÁT ANIMATION
        // Chúng ta không set animator.speed = 0 nữa. 
        // Nhân vật sẽ luôn dậm chân tại chỗ theo hướng cuối cùng.
        animator.speed = 1;
    }

    void FixedUpdate()
    {
        // Di chuyển nhân vật
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}