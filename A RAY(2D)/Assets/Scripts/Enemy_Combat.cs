using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null && Time.time >= lastAttackTime + attackCooldown)
        {
            player.ChangeHealth(-damage);
            lastAttackTime = Time.time;
        }
    }
}