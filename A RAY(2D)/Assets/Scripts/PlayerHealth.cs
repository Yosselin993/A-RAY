using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 6;   // 3 hearts
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player died"); // shows on console 
            gameObject.SetActive(false);
        }
    }
}