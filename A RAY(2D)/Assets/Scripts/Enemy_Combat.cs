using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player == null) return; // if there is no player health, do nothing

        if (player.isDead) return; // if player is dead, stop attacking

        // attack if cooldown is finished
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            player.ChangeHealth(-damage);
            lastAttackTime = Time.time;
        }
    }
}