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
    public GameOverScript gameover;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Collider2D[] colliders;




    void Start()
    {
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
            // gameObject.SetActive(false);
            if (gameover != null)
                gameover.OpenGameoverPanel();
            
        }
    }
}