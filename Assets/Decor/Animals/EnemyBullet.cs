using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 2f; // VERY IMPORTANT

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerBlood playerBlood = collision.GetComponent<PlayerBlood>();
        if (playerBlood != null)
        {
            playerBlood.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
