using NUnit.Framework;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private Rigidbody2D body;

    public Animator anim;


 
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
        }
        
    }
}