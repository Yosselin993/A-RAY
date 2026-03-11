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
        currentHealth -= damage; // taking away damage from enemys health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ensuring it doesnt go below 0 or above

        Debug.Log(gameObject.name + " took " + damage + " damage. HP: " + currentHealth + "/" + maxHealth); // debugging to show on console
        
        //if enemy health has reached 0 or below
        if (currentHealth <= 0)
        {
            GetComponent<EnemyRespawn>().Die();
        }
        
    }
}