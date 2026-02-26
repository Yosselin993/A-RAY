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

        Debug.Log(gameObject.name + " took " + damage + " damage. HP: " + currentHealth + "/" + maxHealth); // debugging 
        
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        
    }
}