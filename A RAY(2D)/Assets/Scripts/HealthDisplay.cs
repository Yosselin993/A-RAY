using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public Sprite halfHeart;
    public Sprite fullHeart;
    public Image[] hearts; // using an array to put as many hearts as wanted

    public PlayerHealth PlayerHealth; // to be able to talk ot one another
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        currentHealth = PlayerHealth.currentHealth;
        maxHealth = PlayerHealth.maxHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = halfHeart;
            }

            if(i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
