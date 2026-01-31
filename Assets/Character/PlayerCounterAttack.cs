using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttack : MonoBehaviour
{
    public float pushRadius = 2.5f;
    public int monsterDamage = 20;
    public float cooldown = 1f;

    private bool canUse = true;

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            && canUse)
        {
            StartCoroutine(Counter());
        }
    }

    IEnumerator Counter()
    {
        canUse = false;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, pushRadius);

        // Prevent hitting the same enemy multiple times
        HashSet<MonsterBlood> hitEnemies = new HashSet<MonsterBlood>();

        foreach (Collider2D hit in hits)
        {
            MonsterBlood monsterBlood =
                hit.GetComponentInParent<MonsterBlood>();

            if (monsterBlood == null) continue;
            if (hitEnemies.Contains(monsterBlood)) continue;

            hitEnemies.Add(monsterBlood);

            Vector2 pushDir =
                (monsterBlood.transform.position - transform.position).normalized;

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

            // Damage (shared)
            monsterBlood.TakeDamage(monsterDamage);
        }

        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    }
}
