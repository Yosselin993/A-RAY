using UnityEngine;

public class EnemyChase2D : MonoBehaviour
{
    [Header("Chase Settings")]
    public float speed = 3f;
    public float detectionRadius = 8f;   // enemy only chases if player is close enough
    public float stopDistance = 0.5f;    // enemy stops when very close (prevents jitter)

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

            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            //switch animation
            anim.SetFloat("horizontalInput", 0);
            return;
        }

        float dist = Vector2.Distance(rb.position, player.position);

        if (dist > detectionRadius)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            anim.SetFloat("horizontalInput", 0);
            return;
        }

        if (dist <= stopDistance)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            anim.SetFloat("horizontalInput", 0);
            return;
        }

        //calc. dir. vectior from enemy to player
        //gives us how far and in what direction the player is, enemy -> player
        Vector2 direction = player.position - transform.position;
        float horizontalInput = Mathf.Sign(direction.x); //move right or move left, horizontal

        Vector2 velocity = rb.linearVelocity; //stores current velocity so we dont accidentlly overwrite y velocity
        velocity.x = horizontalInput * speed; // change only the x movement, horizontal speed
        rb.linearVelocity = velocity; //apply updated velocity 

        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));

        //flips sprite
        if (horizontalInput > -0.01f)
                transform.localScale = new Vector3(1f, 1f, 1f);

        else if (horizontalInput < 0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);


    }

    
    
}