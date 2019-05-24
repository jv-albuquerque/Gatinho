using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject canvasPause = null;
    [SerializeField] private GameObject canvasMainPause = null;
    [SerializeField] private GameObject canvasSettings = null;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        if(!isPaused)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
            canvasMainPause.SetActive(true);
            canvasSettings.SetActive(false);
            canvasPause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            isPaused = false;

            canvasPause.SetActive(false);
        }
    }

    public void Setting()
    {
        canvasMainPause.SetActive(false);
        canvasSettings.SetActive(true);
    }

    public void MainPause()
    {
        canvasMainPause.SetActive(true);
        canvasSettings.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }


}
