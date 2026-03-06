using UnityEngine;
using System.Collections; // needed for respawning

public class EnemyRespawn : MonoBehaviour
{
    [Header("Respawn")]
    public float respawnTime = 3f; // seconds waited for respawning

    private Vector3 startPosition; // enemies start position
    private SpriteRenderer sr; // controls enemy spirte visablity
    private Collider2D col; // controls enemy collision
    private EnemyHealth enemyHealth; // ref for health
    private EnemyMovement enemyMovement; // ref for movement
    private Enemy_Combat enemyCombat; //ref for attack


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position; // saves enemy starting position, this is where it will spawn in later

        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyCombat = GetComponent<Enemy_Combat>();
    }

    // function for when enemy dies
    public void Die()
    {

        StartCoroutine(RespawnCoroutine()); // starts the respwn process
    }

    IEnumerator RespawnCoroutine()
    {
        //gameObject.SetActive(false); //disabled the enemy so it'll disapear from the game
        sr.enabled = false; // hides the enemy sprite
        col.enabled = false; // disables collider
        enemyMovement.enabled = false; //disables movement
        enemyCombat.enabled = false; // disables combat

        yield return new WaitForSeconds(respawnTime); // waits for the num. of secs set in respawntime

        transform.position = startPosition; // moves enemy back to orginal spot

        enemyHealth.currentHealth = enemyHealth.maxHealth; // restores enemies health

        sr.enabled = true; // makes enemy visable again
        col.enabled = true; // enables collider
        enemyMovement.enabled = true; //enables movement
        enemyCombat.enabled = true; // enables combat

        //GetComponent<EnemyHealth>().currentHealth = GetComponent<EnemyHealth>().maxHealth; // resets health

        //gameObject.SetActive(true); // turns enemy back on, appears in the game
    }
}
