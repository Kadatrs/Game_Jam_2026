using UnityEngine;
using System.Collections;

public class PlayerBlood : MonoBehaviour
{
    public int maxBlood = 100;
    public int currentBlood;

    [Header("Damage Effects")]
    public float slowMultiplier = 0.5f;
    public float slowDuration = 1.5f;
    public float invincibleDuration = 1.5f;

    [Header("Healing")]
    public int healAmount = 1;
    public float healInterval = 2f;

    private bool isInvincible = false;
    private PlayerMovement movement;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        currentBlood = maxBlood;
        StartCoroutine(HealOverTime());
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentBlood <= 0) return;

        currentBlood -= damage;
        Debug.Log("Player Blood: " + currentBlood);

        StartCoroutine(SlowPlayer());
        StartCoroutine(Invincibility());

        if (currentBlood <= 0)
        {
            GameOver();
        }
    }

    IEnumerator SlowPlayer()
    {
        float originalSpeed = movement.moveSpeed;
        movement.moveSpeed *= slowMultiplier;

        yield return new WaitForSeconds(slowDuration);

        movement.moveSpeed = originalSpeed;
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    IEnumerator HealOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(healInterval);

            if (currentBlood > 0 && currentBlood < maxBlood)
            {
                currentBlood += healAmount;
                currentBlood = Mathf.Min(currentBlood, maxBlood);
            }
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");

        movement.enabled = false;
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
