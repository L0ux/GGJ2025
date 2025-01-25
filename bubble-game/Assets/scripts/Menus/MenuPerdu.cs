using UnityEngine;
using UnityEngine.UI;

public class MenuPerdu : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private Button recommencerButton;

    [SerializeField]
    private Button quitterButton;


    void Start()
    {
        gameManager = GameManager.Instance;
        recommencerButton.onClick.AddListener(Recommencer);
        quitterButton.onClick.AddListener(Quitter);
        gameManager.SetMenuLoose(this.gameObject);
        this.gameObject.SetActive(false);
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
