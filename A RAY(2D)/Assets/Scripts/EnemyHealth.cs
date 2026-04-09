using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 6;
    public int currentHealth;
    private int baseMaxHealth;

    void Start()
    {
        baseMaxHealth = maxHealth;
        // for difficulty button in main 
        if (GameManager.Instance != null)
        {
            switch (GameManager.Instance.currentDifficulty)
            {
                case Difficulty.Easy:
                    maxHealth = baseMaxHealth - 1;
                    break;

                case Difficulty.Medium:
                    maxHealth = baseMaxHealth;
                    break;

                case Difficulty.Hard:
                    maxHealth = baseMaxHealth + 1;
                    break;
            }
        }
        if (maxHealth < 1) maxHealth = 1;

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