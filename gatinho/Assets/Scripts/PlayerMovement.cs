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
    [SerializeField] private Transform[] groundCheck = null; // object transform that will be used to see if the character is in the ground
    [SerializeField] private LayerMask layerGround = ~0; // Layer that will refer to the ground
    private bool grounded = false; //check if the player is on the ground and if it can jump

    [SerializeField] private Transform[] wallCheck = null; // object transform that will be used to see if the character is in the ground
    [SerializeField] private LayerMask layerWall = ~0; // Layer that will refer to the wall
    private bool walled = false; //check if the player is on the wall
    private bool walledJump = false; //check if the jump was from the wall

    private bool facingRight = true; //store where the player is looking
    private Rigidbody2D rb2D = null;
    private Vector3 velocity = Vector3.zero;

    private GameObject[] specialPlatforms; //there is all the special platforms game objects

    private Animator anim = null; //The character animator

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        specialPlatforms = GameObject.FindGameObjectsWithTag("SpecialPlatform");
    }

    void Update()
    {
        Checkers();
        SetAnim();
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

        if (!grounded)
            for (int i = 0; i < wallCheck.Length; i++)
            {
                if (Physics2D.Linecast(transform.position, wallCheck[i].position, layerWall))
                    walled = true;
            }

        if (grounded || walled)
            walledJump = false;


    }

    private void SetAnim()
    {
        if(grounded)
        {
            anim.SetBool("OnGround", true);
            anim.SetBool("OnWall", false);
            anim.SetBool("IsFalling", false);
        }
        else
        {
            anim.SetBool("OnGround", false);
        }

        if(walled)
        {
            anim.SetBool("IsFalling", false);
        }

        if (!walled && !grounded && rb2D.velocity.y <= 0)
        {
            anim.SetBool("OnWall", false);
            anim.SetTrigger("IsFalling");
        }



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

            //Make it a positive number and a big number to the animator can see if the character is moving
            float tmp = move*move*1000;
            anim.SetFloat("Speed", tmp);
        }
        //case if the player whants to move in the air
        else
        {
            // flip the player where it is moving
            if (rb2D.velocity.x > 0 && !facingRight)
                Flip();
            else if (rb2D.velocity.x < 0 && facingRight)
                Flip();

            float airVelocity = rb2D.velocity.x;

            if (move > 0)
                airVelocity = rb2D.velocity.x + .07f;
            else if (move < 0)
                airVelocity = rb2D.velocity.x - .07f;

            Mathf.Clamp(airVelocity, -1.6f, 1.6f);

            rb2D.velocity = new Vector2(airVelocity, rb2D.velocity.y);
        }


        //case the player is on the wall and not in the ground
        //and is pressing to the wall
        if (!grounded && walled && 
          ((move > 0 && facingRight) || (move < 0 && !facingRight))
          && IsFalling)
        {
            anim.SetBool("OnWall", true);
            rb2D.velocity = new Vector2(0f, -walledDownSpeed/10);
        }


        if(jump)
        {
            if (grounded)
            {
                grounded = false;
                rb2D.AddForce(new Vector2(0f, jumpForce * 10));
                anim.SetBool("OnGround", false);
                anim.SetTrigger("JumpingGround");
            }
            else if(walled)
            {
                rb2D.velocity = new Vector2(0f, 0f);
                float force;
                if (facingRight)
                    force = -1;
                else
                    force = 1;

                rb2D.AddForce(new Vector2(moveSpeed * 10 * force, jumpForce * 10));
                Flip();
                walledJump = true;
                anim.SetTrigger("Jumping");
                anim.SetBool("OnWall", false);
            }
        }
    }

    public void JumpDown()
    {
        if(grounded)
        {
            DesableSpecialPlatform();

            Invoke("EnableSpecialPlatform", 0.5f);
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

    private void DesableSpecialPlatform()
    {
        for (int i = 0; i < specialPlatforms.Length; i++)
        {
            specialPlatforms[i].GetComponent<Collider2D>().enabled = false;
        }
    }

    private void EnableSpecialPlatform()
    {
        for (int i = 0; i < specialPlatforms.Length; i++)
        {
            specialPlatforms[i].GetComponent<Collider2D>().enabled = true;
        }
    }

    public void FindSpecialObject()
    {
        rb2D.gravityScale = 0;
        rb2D.velocity *= 0.1f;
        walledJump = true;
    }

    private bool IsFalling
    {
        get
        {
            return rb2D.velocity.y < 0;
        }
    }
}
