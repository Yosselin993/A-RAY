using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 6;   // 3 hearts
    public int currentHealth;
    public bool isDead = false; // this will keep track of players death

    // For death animation, currently working on it.
    [Header("Death")]
    public Animator anim;
    public float deathDelay = 1f; // how long to wait before disabling player, to showcase death
    public GameOverScrpit gameover;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Collider2D[] colliders;




    void Start()
    {
        // for difficulty button in main 
        if (GameManager.Instance != null)
        {
            switch (GameManager.Instance.currentDifficulty)
            {
                case Difficulty.Easy:
                    maxHealth = 8;
                    break;

                case Difficulty.Medium:
                    maxHealth = 6;
                    break;

                case Difficulty.Hard:
                    maxHealth = 4;
                    break;
            }
        }
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        //if already dead, ignore any more damage (Do we want to add healing?)
        if (isDead) return;

        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0 && !isDead)  
        {
            isDead = true; //died
            Debug.Log("Player died"); // shows on console 
            // gameover.OpenGameoverPanel();
            
            // Saves this run to leaderboard
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveCurrentRunToLeaderboard();
            }

            gameObject.SetActive(false);
            if (gameover != null)
            {
                Debug.Log("Opening GameOver Panel");
                gameover.OpenGameoverPanel();
            }
            else
            {
                Debug.Log("GameOverScript NOT assigned!");
            }
                        
        }
    }
}