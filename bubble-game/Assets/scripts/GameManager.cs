using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private GameObject menuPause;
    private bool isGamePaused = false;
    private SceneManager sceneManager;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        sceneManager = SceneManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    private void PauseGame()
    {
        menuPause.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        menuPause.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1f; 
    }

    public void ReloadGame()
    {
        ResumeGame();
        sceneManager.ReloadCurrentScene();
    }

    public void QuitGame()
    {
        sceneManager.QuitGame();
    }

    public void SetMenuManager(GameObject menu)
    {
        menuPause = menu;
    }

}
