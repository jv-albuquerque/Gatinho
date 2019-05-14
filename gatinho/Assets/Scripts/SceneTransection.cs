using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransection : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.GetComponent<PlayerMovement>().FindSpecialObject();
            //Make particles go out from the object
            //make the camera zoom into the player
            //change to the next stage
            Invoke("ChangeScene", 1f);
        }
    }

    private void ChangeScene()
    {
        //Go to the load screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
