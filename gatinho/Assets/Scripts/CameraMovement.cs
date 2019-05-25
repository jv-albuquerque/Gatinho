using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform leftLimit = null; // left limit where the camera can go
    [SerializeField] private Transform rightLimit = null; // right limit where the camera can go

    [SerializeField] private Transform pushleft = null; // left limit to the player pushs the camera
    [SerializeField] private Transform pushRight = null; // right limit to the player pushs the camera

    private float difRightLeft; // gets the diference between pushRight and PushLeft

    private Transform playerTransform = null;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        difRightLeft = pushleft.position.x - pushRight.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // if the player is out the limits to push the camera
        if(playerTransform.position.x <= pushleft.position.x ||
           playerTransform.position.x >= pushRight.position.x)
            MoveCamera();
    }

    private void MoveCamera()
    {
        //the function to move the camera
        //if the player is pushing from the left
        if(playerTransform.position.x <= pushleft.position.x)
        {
            transform.position = new Vector3(playerTransform.position.x - difRightLeft/2, transform.position.y, transform.position.z);
        }
        //if the player is pushing from the right
        else
        {
            transform.position = new Vector3(playerTransform.position.x + difRightLeft / 2, transform.position.y, transform.position.z);
        }


        //if the camera wants to go out its limits
        if(transform.position.x > rightLimit.position.x)
            transform.position = new Vector3(rightLimit.position.x, transform.position.y, transform.position.z);
        else if (transform.position.x < leftLimit.position.x)
            transform.position = new Vector3(leftLimit.position.x, transform.position.y, transform.position.z);
    }
}
