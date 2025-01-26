using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public float waitAfterNextLevel = 2f;
    private GameObject menuPause;
    private bool isGamePaused = false;
    private bool isLevelEnded = false;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        SceneManager.Instance.LoadNextScene();
    }

    public void WinGame()
    {
        isLevelEnded = true;
        menuPause.SetActive(false);
        StartCoroutine(WaitAndReloadLevel(waitAfterNextLevel));
    }

    public void LooseGame()
    {
        isLevelEnded = true;
        menuPause.SetActive(false);
        StartCoroutine(WaitAndLoadNextLevel(waitAfterNextLevel));
    }

    private IEnumerator WaitAndLoadNextLevel(float waitTime)
    {
       
        yield return new WaitForSeconds(waitTime);
        ReloadGame();
    }

    private IEnumerator WaitAndReloadLevel(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        NextLevel();
    }

    public void ReloadGame()
    {
        isLevelEnded = false;
        ResumeGame();
        SceneManager.Instance.ReloadCurrentScene();
    }

    public void QuitGame()
    {
        SceneManager.Instance.QuitGame();
    }


    //Setter
    public void SetMenuPause(GameObject menu)
    {
        menuPause = menu;
    }

}
