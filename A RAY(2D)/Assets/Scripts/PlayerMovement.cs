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
    public int attackDamage = 1;       // player attack damage 

    public float attackCooldown = 0.5f;  // seconds between attacks
    private float nextAttackTime = 0f;   // when you're allowed to attack again
    private bool isAttacking = false;    // helps prevent weird repeats

    // trying to fix attack direction, feedback request
    [Header("Attack Direction")]
    public float attackPointOffsetX = 1f; // this is for how far attack point should sit from player center
    private bool isFacingRight = true; // this will store the last direction of the player faced, naturally its already set to the right side

    // jumping, feedback request
    [Header("Jump")]
    public float jumpforce = 3f; // this will be how strong the player jump is
    public float jumpCooldown = 3f; // this will be for the time between jumps
    private float nextJumpTime = 0f; // this is when the player jumps again

 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        UpdateAttackPointPosition(); // makes sure the atatck point is on the right side
    }

    // Update is called once per frame
    void Update()
    {

        // float horizontalInput = Input.GetAxis("Horizontal"); // for AWDS or arrow keys
        // switching to GetAxisRaw as it gives instant left/right inpput
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // for AWDS or arrow keys
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y); // moving player left and right

        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput)); // walking animation
 

        //Flip player when facing left/right.
        // if (horizontalInput > -0.01f)
        //     transform.localScale = new Vector3(1f, 1f, 1f);

        // else if (horizontalInput < 0.01f)
        //     transform.localScale = new Vector3(-1f, 1f, 1f);


        // trying new logic for fliping + storing direction for isFacingRight
        // This was because feedback, was told they couldnt "attack backwards" or couldnt "attack left" 
        if (horizontalInput > 0)
        {
            isFacingRight = true; // remebers the player is facing right
            transform.localScale = new Vector3(1f, 1f, 1f); // face right? i think, gotta test
            UpdateAttackPointPosition(); // moves attack point to the rigght side
        }
        else if (horizontalInput < 0)
        {
            isFacingRight = false; // remembers the player facing left
            transform.localScale = new Vector3(-1f, 1f, 1f); // face left
            UpdateAttackPointPosition(); // moves the attack point to left side
        }

        // // jump
        // if (Input.GetKeyDown(KeyCode.Space))
        //     body.linearVelocity = new Vector2(body.linearVelocity.x, speed);

        // jump, updated feeback request
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time < nextJumpTime) return; // checks if the cooldown is over

            nextJumpTime = Time.time + jumpCooldown;

            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpforce);
        }


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

    void UpdateAttackPointPosition()
    {
        if (attackPoint == null) return;

        float direction = isFacingRight ? 1f : -1f;

        attackPoint.localPosition = new Vector3(
            direction * attackPointOffsetX,
            attackPoint.localPosition.y,
            attackPoint.localPosition.z
        );
    }


    public void DealDamage() // function for the anim. event for the attack animation for player
    {
        Debug.Log("DealDamage fired!"); // debugging

        if (!isAttacking) { return; } // if player is not attacking, stop
        if (attackPoint == null) { return; } // if no attack point is assigned, stop

        // finding all colliders in the attack range that are on the enemy layer
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        Debug.Log("Hit count: " + hitEnemies.Length);  // debug, console show 

        // going through each enemy collider 
        foreach (Collider2D c in hitEnemies)
        {

            // parent-safe (common when collider is on child) -- need to fix
            EnemyHealth enemyHealth = c.GetComponentInParent<EnemyHealth>(); // trying to find enemy health on object

            if (enemyHealth != null)
            {
                Debug.Log("Found EnemyHealth on: " + enemyHealth.gameObject.name + " -> dealing damage"); //debugging
                enemyHealth.TakeDamage(attackDamage); // if enemy health exists, deal damage
            }
            else
            {
                Debug.Log("No EnemyHealth found on this collider or its parents."); //debugging, console shows
            }
        }
    }

    // Called by an Animation Event near the end of the attack animation
    public void EndAttack()
    {
        isAttacking = false; // shows the attack as finished
    }


    // for debugging, shows attack range in Scene view when player is selected
    void OnDrawGizmosSelected()
    {
        // if attack point is missing, do nothing
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red; // red circle to show range
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}