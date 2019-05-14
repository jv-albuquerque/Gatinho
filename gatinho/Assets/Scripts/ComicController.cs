using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicController : MonoBehaviour
{
    [Header("Frames")]
    [SerializeField] private GameObject[] frames;
    private float[] framesY;
    [Header("Parameters")]
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float speedUp = 10;
    [SerializeField] private float cooldownToNextFrame = 1;

    private bool anime = false; //if can do the animation of the frames moving up
    private bool lastFrame = false; //if the last frame is showned


    private Cooldown cd; // used to count when the next frame will be showned
    private int count = 0; // Variable thats store the frame that will be showned

    // Start is called before the first frame update
    void Start()
    {
        cd = new Cooldown(cooldownToNextFrame);
        framesY = new float[frames.Length];

        for (int i = 0; i < frames.Length; i++)
        {
            frames[i].SetActive(false);
            framesY[i] = frames[i].transform.position.y;
        }

        NextFrame();
        cd.Start();
    }

    void FixedUpdate()
    {
        if (!lastFrame)
        {
            if ((Input.anyKeyDown || cd.IsFinished()) && !anime)
            {
                cd.Reset();
                NextFrame();

                if (count == frames.Length)
                    anime = true;
            }
            else if (anime)
            {
                MoveFramesUp();
            }
        }
        else
        {
            // TODO: Read the mouse wheel to move the frames up n down
        }
    }

    private void NextFrame()
    {
        if (count >= frames.Length)
        {
            return;
        }

        frames[count].SetActive(true);
        count++;

        if (count < frames.Length && frames[count].transform.position.y < -200)
            anime = true;

    }

    private void MoveFramesUp()
    {
        for (int i = 0; i < frames.Length; i++)
        {
            Vector3 target = new Vector3(frames[i].transform.position.x, frames[i].transform.position.y + speedUp, frames[i].transform.position.y);

            frames[i].transform.position = target;
        }

        if (frames[count - 1].transform.position.y >= centerPoint.position.y)
        {
            cd.Reset();
            anime = false;
            if(count == frames.Length)
                lastFrame = true;
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
