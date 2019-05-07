using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private float horizontalMove;
    private bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input left and right
        horizontalMove = Input.GetAxisRaw("Horizontal");

        // Get the input to jump
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }


    void FixedUpdate()
    {
        // than call the fuction to move
        playerMovement.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
