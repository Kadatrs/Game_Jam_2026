using UnityEngine;
using System.Collections;

public class EnemyRangedMonster : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 6f;
    public float fireCooldown = 0.5f;
    public float bulletSpeed = 6f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private bool isKnockedBack;
    private float knockbackEndTime;
    public float knockbackDuration = 0.2f;

    private Transform player;
    private Rigidbody2D rb;
    private float lastFireTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        if (player == null) return;
        if (isKnockedBack) return; // 🔴 IMPORTANT


        float distance = Vector2.Distance(rb.position, player.position);

        if (distance <= detectionRange)
        {
            TryShoot();
            Vector2 dir = (player.position + transform.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void ApplyKnockback(Vector2 direction)
    {
        StopAllCoroutines();
        StartCoroutine(KnockbackRoutine(direction));
    }

    IEnumerator KnockbackRoutine(Vector2 direction)
    {
        isKnockedBack = true;

        rb.velocity = Vector2.zero;

        float knockbackSpeed = 6f; // same tuning
        rb.velocity = direction.normalized * knockbackSpeed;

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }



    void TryShoot()
    {
        if (Time.time - lastFireTime < fireCooldown) return;

        lastFireTime = Time.time;

        Vector2 direction = (player.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }
}