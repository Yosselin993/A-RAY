using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public Sprite fullHeart; // two health points
    public Sprite halfHeart; // one health point

    public Image[] hearts; // array for UI img. hearts, each img. rep. one heart slot 

    void Update()
    {
        int health = playerHealth.currentHealth; // grabing players health value 

        // loop through every heart slot in UI
        for (int i = 0; i < hearts.Length; i++)
        {
            //each heart rep. two health points
            int heartValue = (i * 2) + 2;

            if (health >= heartValue) //If players health is equal to or greater than this heart's full value
            {
                hearts[i].enabled = true; // makes sure heart is visible 
                hearts[i].sprite = fullHeart; // shows full heart spirte
            }
            else if (health == heartValue - 1) //If players health is exactly one less than this heart's full value
            {
                hearts[i].enabled = true; // makes sure heart is vis.
                hearts[i].sprite = halfHeart; // shows half heart sprite
            }
            else // If players health is lower than this heart's range, heart should not appear
            {
                hearts[i].enabled = false; // completely hides heart so it wont show
            }
        }
    }
}