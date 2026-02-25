using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 6;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}