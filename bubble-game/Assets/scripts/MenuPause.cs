using UnityEngine;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{

    private GameManager gameManager;

    [SerializeField]
    private Button reprendreButton;

    [SerializeField]
    private Button recommencerButton;

    [SerializeField]
    private Button quitterButton;


    void Start()
    {
        gameManager = GameManager.Instance;
        reprendreButton.onClick.AddListener(Reprendre);
        recommencerButton.onClick.AddListener(Recommencer);
        quitterButton.onClick.AddListener(Quitter);
        gameManager.SetMenuManager(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void Reprendre()
    {
        gameManager.ResumeGame();
    }

    public void Recommencer()
    {
        gameManager.ReloadGame();
    }

    public void Quitter()
    {
        gameManager.QuitGame();
    }
}
