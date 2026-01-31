using UnityEngine;

public class MonsterBlood : MonoBehaviour
{
    public int maxBlood = 50;
    public int currentBlood;

    void Start()
    {
        currentBlood = maxBlood;
    }

    public void TakeDamage(int damage)
    {
        currentBlood -= damage;
        Debug.Log("Monster Blood: " + currentBlood);

        if (currentBlood <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Monster Died");
        Destroy(gameObject);
    }
}
