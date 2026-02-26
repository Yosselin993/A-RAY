using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    private Rigidbody2D body;

    [Header("References")]
    public Animator anim;

    [Header("Combat")]
    public Transform attackPoint;      // where the attack is centered
    public float attackRange = 1f;     // radius of attack
    public LayerMask enemyLayers;      // only detect enemies

    public float attackCooldown = 0.5f;  // seconds between attacks
    private float nextAttackTime = 0f;   // when you're allowed to attack again
    private bool isAttacking = false;    // helps prevent weird repeats


 
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
            // cooldown check
            if (Time.time < nextAttackTime) return;

            nextAttackTime = Time.time + attackCooldown;

            anim.SetTrigger("Attack");
            isAttacking = true; // prevent weird repeats, so since attack this will be on then off
        }
        
    }


    public void DealDamage()
    {
        Debug.Log("DealDamage fired!"); // debugging

        if (!isAttacking) { return; }
        if (attackPoint == null) { return; }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        Debug.Log("Hit count: " + hitEnemies.Length);

        foreach (Collider2D c in hitEnemies)
        {

            // parent-safe (common when collider is on child)
            EnemyHealth enemyHealth = c.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                Debug.Log("Found EnemyHealth on: " + enemyHealth.gameObject.name + " -> dealing 1 damage"); //debugging
                enemyHealth.TakeDamage(1);
            }
            else
            {
                Debug.Log("No EnemyHealth found on this collider or its parents."); //debugging
            }
        }
    }

    // Called by an Animation Event near the end of the attack animation
    public void EndAttack()
    {
        isAttacking = false;
    }



    // for debugging, shows attack range in Scene view when player is selected
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}