using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed and Forces")]
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float moveSpeed = 40f;
    [SerializeField] private float jumpForce = 40f;
    [SerializeField] private float walledDownSpeed = 2; // How quickly the object fall down when is walled

    [Header("Checkers")]
    [Space]
    [SerializeField] private Transform[] groundCheck; // object transform that will be used to see if the character is in the ground
    [SerializeField] private LayerMask layerGround; // Layer that will refer to the ground
    private bool grounded = false; //check if the player is on the ground and if it can jump

    [SerializeField] private Transform[] wallCheck; // object transform that will be used to see if the character is in the ground
    [SerializeField] private LayerMask layerWall; // Layer that will refer to the wall
    private bool walled = false; //check if the player is on the wall
    private bool walledJump = false; //check if the jump was from the wall
    [SerializeField] private float wallJumpForce = 50;

    private bool facingRight = true; //store where the player is looking
    private Rigidbody2D rb2D;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Checkers();
    }

    //Verify all checkers to verify if the player is on ground or on the wall
    private void Checkers()
    {
        grounded = false;
        walled = false;

        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (Physics2D.Linecast(transform.position, groundCheck[i].position, layerGround))
                grounded = true;
        }

        if(!grounded)
            for (int i = 0; i < wallCheck.Length; i++)
            {
                if (Physics2D.Linecast(transform.position, wallCheck[i].position, layerWall))
                    walled = true;
            }

        if (grounded || walled)
            walledJump = false;
    }

    public void Move(float move, bool jump)
    {
        if(!walledJump)
        {
            // flip the player where it is moving
            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * moveSpeed * 10, rb2D.velocity.y);
            // And then smoothing it out and applying it to the character
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        //case the player is on the wall and not in the ground
        //and is pressing to the wall
        if(!grounded && walled && 
          ((move > 0 && facingRight) || (move < 0 && !facingRight)))
        {
            rb2D.velocity = new Vector2(0f, -walledDownSpeed/10);
            if (jump)
            {
                rb2D.velocity = new Vector2(0f,0f);
                float force;
                if (facingRight)
                    force = -1;
                else
                    force = 1;
                rb2D.AddForce(new Vector2(moveSpeed * 10 * force, jumpForce * 10));
                Flip();
                walledJump = true;
            }

        }


        if(jump && grounded)
        {
            grounded = false;
            rb2D.AddForce(new Vector2(0f, jumpForce*10));
        }
    }

    private void Flip()
    {
        // Switch the way the player is facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.tag.Equals("Ground"))
            grounded = true;
            */
    }
}
