using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  
  public int currentHealth;
  public int maxHealth;

  public SpriteRenderer PlayerSr;
  public PlayerMovement PlayerMovement;


  public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}