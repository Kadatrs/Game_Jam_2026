using UnityEngine;
using System.Collections;

public class EnemyMonster : MonoBehaviour{
    public float moveSpeed = 2f;
    public float detectionRange = 3f;
    public int damage = 10;
    public float attackCooldown = 1.2f;

    private bool isKnockedBack;
    private float knockbackEndTime;
    public float knockbackDuration = 0.2f;


    private Transform player;
    private Rigidbody2D rb;
    private float lastAttackTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //void FixedUpdate()
    //{
    //    if (player == null) return;

    //    float distance = Vector2.Distance(rb.position, player.position);

    //    if (distance <= detectionRange)
    //    {
    //        Vector2 dir = (player.position - transform.position).normalized;
    //        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    //    }
    //}

    void FixedUpdate()
    {
        if (player == null) return;
        if (isKnockedBack) return; // 🔴 IMPORTANT

        float distance = Vector2.Distance(rb.position, player.position);

        if (distance <= detectionRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
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

        float knockbackSpeed = 6f; // 🔧 TUNE THIS
        rb.velocity = direction.normalized * knockbackSpeed;

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }



    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        PlayerBlood playerBlood = collision.gameObject.GetComponent<PlayerBlood>();
        if (playerBlood != null)
        {
            playerBlood.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}