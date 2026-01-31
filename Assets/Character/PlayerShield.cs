using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
    public float shieldRadius = 2f;
    public float pushForce = 5f;
    public float shieldDuration = 1f;

    private bool shieldActive = false;
    private float lastShieldTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !shieldActive)
        {
            StartCoroutine(ActivateShield());
        }
    }

    IEnumerator ActivateShield()
    {
        shieldActive = true;
        lastShieldTime = Time.time;

        float endTime = Time.time + shieldDuration;

        while (Time.time < endTime)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                transform.position, shieldRadius);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Rigidbody2D enemyRb = hit.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        Vector2 pushDir =
                            (hit.transform.position - transform.position).normalized;
                        enemyRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
                    }
                }
            }

            yield return null;
        }

        shieldActive = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, shieldRadius);
    }
}
