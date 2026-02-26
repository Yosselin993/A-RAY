using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Chase Settings")]
    public float speed = 3f;
    public float detectionRadius = 8f;   // enemy only chases if player is close enough
    //public float stopDistance = 0.2f;    // enemy stops when very close (prevents jitter)
    public float attackRange = 1.2f; // distance at which enemy attacks

    public Animator anim;
    

    Transform player;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null) 
            player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            // Try to reacquire player (useful if you respawn / destroy-recreate)
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) 
                player = p.transform;

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            //switch animation
            anim.SetFloat("horizontalInput", 0);
            return;
        }

        float dist = Vector2.Distance(rb.position, player.position);

        if (dist > detectionRadius)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetFloat("horizontalInput", 0);
            return;
        }

        // Calculate direction to player
        Vector2 direction = player.position - transform.position;

        // Always face the player (even when attacking)
        if (direction.x > 0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (direction.x < -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        // Now check attack
        if (dist <= attackRange)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetTrigger("Attack");
            return;
        }

        // Movement logic
        float horizontalInput = Mathf.Sign(direction.x);

        Vector2 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * speed;
        rb.linearVelocity = velocity;

        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));


    }

    
    
}