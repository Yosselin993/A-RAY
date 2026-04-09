using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private int baseDamage; // for difficulty

    // enemy attack range, feedback 
    [Header("Attack Range")]
    public Transform attackPoint; // this is for the point of where the enemy attack is centered
    public float attackRange = 1.2f; // this is for how far the enemy can hit-- gotta mess with this
    public LayerMask playerLayer; // this is so the enemy can detect the player

    // we kind of want this like the PlayerMovement.cs soooo
    private bool isAttacking = false; 

    void Start()
    {
        // for difficulty button in main 
        baseDamage = damage;

        if (GameManager.Instance != null)
        {
            switch (GameManager.Instance.currentDifficulty)
            {
                case Difficulty.Easy:
                    damage = baseDamage;
                    break;

                case Difficulty.Medium:
                    damage = baseDamage;
                    break;

                case Difficulty.Hard:
                    damage = baseDamage + 1;
                    break;
            }
        }
    }

    public void StartAttack()
    {
        // if cooldown has not finished, stop
        if (Time.time < lastAttackTime + attackCooldown) return;

        isAttacking = true;
    }

    public void DealDamage()
    {
        if (!isAttacking) return;

        //if attack point is missing, stop
        if (attackPoint == null) return;


        //checking if the player is inside the attack range using OverlapCircle
        Collider2D hitPlayer = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRange,
            playerLayer
        );

        // if no player is found in attack range, do nothin
        if (hitPlayer == null) return;

        // getting the PlayerHealth from hit
        PlayerHealth player = hitPlayer.GetComponent<PlayerHealth>();

        if (player == null) return;

        if (player.isDead) return;

        // damage
        player.ChangeHealth(-damage);
        lastAttackTime = Time.time;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    // for debugging, like PlayerMovement, will be using to show attack range 
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



    // old code
    //     private void OnCollisionStay2D(Collision2D collision)
    //     {
    //         PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

    //         if (player == null) return; // if there is no player health, do nothing

    //         if (player.isDead) return; // if player is dead, stop attacking

    //         // attack if cooldown is finished
    //         if (Time.time >= lastAttackTime + attackCooldown)
    //         {
    //             player.ChangeHealth(-damage);
    //             lastAttackTime = Time.time;
    //         }
    //     }
}