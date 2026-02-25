using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private Rigidbody2D body;

    public Animator anim;

    public Transform attackPoint;      // where the attack is centered
    public float attackRange = 1f;     // radius of attack
    public LayerMask enemyLayers;      // only detect enemies


 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
 
        //Flip player when facing left/right.
        if (horizontalInput > -0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);

        else if (horizontalInput < 0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        // jump
         if (Input.GetKey(KeyCode.Space))
            body.linearVelocity = new Vector2(body.linearVelocity.x, speed);

        //attack
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("Attack");
            Attack();
        }
        
    }

    void Attack()
    {
        // Detect all colliders inside attack circle that are on the Enemy layer
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1); // 1 = half heart
            }
        }
    }

    // for debugging
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}