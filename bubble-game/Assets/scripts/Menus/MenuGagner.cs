using UnityEngine;
using UnityEngine.UI;

public class MenuGagner : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private Button suivantBouton;

    [SerializeField]
    private Button recommencerButton;

    [SerializeField]
    private Button quitterButton;


    void Start()
    {
        gameManager = GameManager.Instance;
        suivantBouton.onClick.AddListener(Suivant);
        recommencerButton.onClick.AddListener(Recommencer);
        quitterButton.onClick.AddListener(Quitter);
        this.gameObject.SetActive(false);
    }

    private void Suivant()
    {
        gameManager.NextLevel();
    }

    private void Recommencer()
    {
        gameManager.ReloadGame();
    }

    private void Quitter()
    {
        gameManager.QuitGame();
    }
}
