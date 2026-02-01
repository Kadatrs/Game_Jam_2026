using UnityEngine;
using System.Collections;

public class FinalBoss : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public int bulletCount = 10;
    public float detectionRange = 6f;
    public float fireRate = 2f; // time T

    private Coroutine fireRoutine;

    public float slowMultiplier = 0.5f; // 50% speed
    private float originalPlayerSpeed;
    private bool playerSlowed;

    private Transform player;
    private Rigidbody2D rb;
    private bool isKnockedBack;
    public float knockbackDuration = 0.2f;
    public int damage = 25;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    [Header("Danger Zones")]
    public GameObject dangerCirclePrefab;
    public int dangerCircleCount = 5;
    public float dangerCircleRadius = 3f;

    [Header("Danger Zone Rotation")]
    public float dangerZoneRotationSpeed = 10f; // degrees per second
    private float dangerZoneAngleOffset;


    private GameObject[] dangerCircles;
    private bool zonesSpawned;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(rb.position, player.position);

        if (distance <= detectionRange && fireRoutine == null && !isKnockedBack)
        {
            SpawnDangerZones();
            ApplySlow();
            fireRoutine = StartCoroutine(FireCircleRoutine());
        }
        else if ((distance > detectionRange || isKnockedBack) && fireRoutine != null)
        {
            RemoveDangerZones();
            RemoveSlow();
            StopCoroutine(fireRoutine);
            fireRoutine = null;
        }
        if (zonesSpawned)
        {
            RotateDangerZones();
        }

    }

    void SpawnDangerZones()
    {
        if (zonesSpawned) return;

        dangerCircles = new GameObject[dangerCircleCount];

        float angleStep = 360f / dangerCircleCount;
        float angle = 0f;

        for (int i = 0; i < dangerCircleCount; i++)
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(
                Mathf.Cos(rad),
                Mathf.Sin(rad)
            ) * dangerCircleRadius;

            GameObject zone = Instantiate(
                dangerCirclePrefab,
                (Vector2)transform.position + offset,
                Quaternion.identity
            );

            dangerCircles[i] = zone;
            angle += angleStep;
        }

        zonesSpawned = true;
    }

    void RotateDangerZones()
    {
        dangerZoneAngleOffset += dangerZoneRotationSpeed * Time.deltaTime;

        float angleStep = 360f / dangerCircleCount;
        float angle = dangerZoneAngleOffset;

        for (int i = 0; i < dangerCircleCount; i++)
        {
            if (dangerCircles[i] == null) continue;

            float rad = angle * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(
                Mathf.Cos(rad),
                Mathf.Sin(rad)
            ) * dangerCircleRadius;

            dangerCircles[i].transform.position =
                (Vector2)transform.position + offset;

            angle += angleStep;
        }
    }


    void RemoveDangerZones()
    {
        if (!zonesSpawned) return;

        foreach (GameObject zone in dangerCircles)
        {
            if (zone != null)
                Destroy(zone);
        }

        zonesSpawned = false;
    }



    void ApplySlow()
    {
        if (playerSlowed) return;

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm == null) return;

        originalPlayerSpeed = pm.moveSpeed;
        pm.moveSpeed *= slowMultiplier;
        playerSlowed = true;
    }

    void RemoveSlow()
    {
        if (!playerSlowed) return;

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm == null) return;

        pm.moveSpeed = originalPlayerSpeed;
        playerSlowed = false;
    }


    IEnumerator FireCircleRoutine()
    {
        while (true)
        {
            FireCircle();
            yield return new WaitForSeconds(fireRate);
        }
    }


    void FireCircle()
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float radian = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

            angle += angleStep;
        }
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
