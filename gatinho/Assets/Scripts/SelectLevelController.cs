using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelController : MonoBehaviour
{

    private int unlock = 1;

    [SerializeField] private GameObject LockLevel2 = null;
    [SerializeField] private GameObject LockLevel3 = null;
    [SerializeField] private GameObject LockLevel4 = null;


    // Start is called before the first frame update
    void Start()
    {
        if(unlock >= 2)
        {
            LockLevel2.SetActive(false);

            if (unlock >= 3)
            {
                LockLevel3.SetActive(false);

                if (unlock >= 4)
                    LockLevel4.SetActive(false);
            }

        }
    }

    public void Level1()
    {
        SceneManager.LoadScene("Comic0");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Level4()
    {
        SceneManager.LoadScene("Level4");
    }
}
