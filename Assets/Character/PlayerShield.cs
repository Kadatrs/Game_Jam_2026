using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
    public float shieldRadius = 2f;
    public float pushForce = 8f;
    public float cooldown = 1f;

    private bool canUse = true;

    public void TryActivateShield()
    {
        if (!canUse) return;
        StartCoroutine(ShieldPulse());
    }

    IEnumerator ShieldPulse()
    {
        canUse = false;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, shieldRadius);

        foreach (Collider2D hit in hits)
        {
            MonsterBlood monsterBlood =
                hit.GetComponentInParent<MonsterBlood>();

            if (!hit.CompareTag("Enemy")|| (monsterBlood == null)) continue;

            Rigidbody2D enemyRb = hit.GetComponent<Rigidbody2D>();
            if (enemyRb == null) continue;

            Vector2 pushDir =
                (monsterBlood.transform.position - transform.position).normalized;

            // 🔑 THIS IS THE IMPORTANT PART
            //enemyRb.velocity = Vector2.zero;
            //enemyRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);

            // Melee enemy
            EnemyMonster enemy =
                monsterBlood.GetComponent<EnemyMonster>();
            if (enemy != null)
            {
                enemy.ApplyKnockback(pushDir);
            }

            // Ranged enemy
            EnemyRangedMonster rangedEnemy =
                monsterBlood.GetComponent<EnemyRangedMonster>();
            if (rangedEnemy != null)
            {
                rangedEnemy.ApplyKnockback(pushDir);
            }
            yield return new WaitForSeconds(cooldown);
            canUse = true;
        }

        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, shieldRadius);
    }
}
