using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager Instance { get; private set; }


    [SerializeField]
    private GameObject menuPrincipale;
    [SerializeField]
    private GameObject choixNiveau;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        menuPrincipale.SetActive(true);
        choixNiveau.SetActive(false);
    }


    public void OnJouer()
    {
        menuPrincipale.SetActive(false);
        choixNiveau.SetActive(true);
    }

    public void OnSelectionNiveau(int niveau)
    {
        Debug.Log("Choix du niveau");
        Debug.Log(niveau);
    }

}
