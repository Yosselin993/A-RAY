using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 6;
    public int currentHealth;

    [Header("Score")]
    public int scoreValue = 100; // how many points this enemy gives

    private int baseMaxHealth;   // stores the original health set in Inspector
    private bool hasGivenScore = false; // prevents score from being added twice per life
    private bool initializedBaseHealth = false;

    void OnEnable()
    {
        // Save the original inspector health only once
        if (!initializedBaseHealth)
        {
            baseMaxHealth = maxHealth;
            initializedBaseHealth = true;
        }

        // baseMaxHealth = maxHealth;

         // Reset score flag
        hasGivenScore = false;

        maxHealth = baseMaxHealth;

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
            // Add score only once
            if (!hasGivenScore && GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
                hasGivenScore = true;

                Debug.Log(gameObject.name + " gave score: " + scoreValue); //debug
            }

            // respawn
            GetComponent<EnemyRespawn>().Die();
            GetComponent<EnemyHealth>().OnEnable(); // resets the scoring 
        }
        
    }
}