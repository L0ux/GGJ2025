using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource; // Référence à l'AudioSource pour la musique
    public AudioClip musicClip;     // Clip de musique à jouer en boucle
    private AudioClip changeClip;   // Clip temporaire pour le changement de musique
    private static SoundManager instance; // Singleton
    
    private float baseVolume = 0.7f;    // Volume de base par défaut
    private float currentVolume;
    
    private bool isFadingOut = false;
    private bool isFadingIn = false;

    private float speed;

    void Awake()
    {
        // Gestion du singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        currentVolume = baseVolume;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.volume = baseVolume;
            musicSource.Play();
        }
    }

    void Update()
    {
        if (isFadingOut)
        {
            currentVolume = Mathf.Clamp(currentVolume - (Time.deltaTime * speed), 0f, baseVolume);
            SetVolume(currentVolume);

            if (currentVolume <= 0f)
            {
                isFadingOut = false;
                isFadingIn = true;
                musicSource.clip = changeClip;
                musicSource.Play();
            }
        }

        if (isFadingIn)
        {
            currentVolume = Mathf.Clamp(currentVolume + (Time.deltaTime * speed), 0f, baseVolume);
            SetVolume(currentVolume);

            if (currentVolume >= baseVolume)
            {
                isFadingIn = false;
            }
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

    public void ChangeMusic(AudioClip clip)
    {
        if (clip == null || clip == musicSource.clip) return;
        changeClip = clip;
        isFadingOut = true;
    }

    public void SetChangeSpeed(float newSpeed){
        speed = newSpeed;
    }

    public void SetBaseVolume(float volume){
        baseVolume = volume;
    }
}
