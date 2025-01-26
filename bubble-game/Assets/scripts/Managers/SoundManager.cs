using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;    // Référence à l'AudioSource pour la musique
    public AudioClip musicClip;        // Clip de musique à jouer en boucle
    private static SoundManager instance; // Instance statique pour gérer le singleton


    public SceneManager sceneManager;

    void Awake()
    {
        // Vérifie si une instance existe déjà
        if (instance != null && instance != this)
        {
            // Si une instance existe déjà, détruire cet objet (évite les doublons)
            Destroy(gameObject);
        }
        else
        {
            if (sceneManager.IsLastScene())
            {
                PlayMusicForLastScene();
            }
            instance = this;
            DontDestroyOnLoad(gameObject);  // Cela permet de garder l'objet lors du changement de scène
        }

    }

    void Start()
    {
          
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;   // Active la boucle
            musicSource.Play();        // Joue la musique
        }
    }

   
    public void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    private void PlayMusicForLastScene()
    {
        StopMusic();
        musicSource.clip = musicClip;
        musicSource.loop = false; 
        musicSource.Play();
    }
}
