using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private GameObject menuPause;
    private GameObject menuWin;
    private GameObject menuLoose;
    private bool isGamePaused = false;
    private bool isLevelEnded = false;
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
        if (Input.GetKeyDown(KeyCode.Escape) && !isLevelEnded)
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

    public void NextLevel()
    {
        sceneManager.LoadNextScene();
    }

    public void WinGame()
    {
        isLevelEnded = true;
        menuPause.SetActive(false);
        menuWin.SetActive(true);
        menuLoose.SetActive(false);
    }

    public void LooseGame()
    {
        isLevelEnded = true;
        menuPause.SetActive(false);
        menuWin.SetActive(false);
        menuLoose.SetActive(true);
    }

    public void ReloadGame()
    {
        isLevelEnded = false;
        ResumeGame();
        sceneManager.ReloadCurrentScene();
    }

    public void QuitGame()
    {
        sceneManager.QuitGame();
    }


    //Setter
    public void SetMenuPause(GameObject menu)
    {
        menuPause = menu;
    }

    public void SetMenuWin(GameObject menu)
    {
        menuWin = menu;
    }

    public void SetMenuLoose(GameObject menu)
    {
        menuLoose = menu;
    }

}
