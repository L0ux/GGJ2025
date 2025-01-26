using UnityEngine;
using UnityEngine.Video;

public class EndManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip videoClip;
    private string videoUrl = "https://drive.google.com/file/d/1qIg8lAjElf297wywBfXOiphIvY-_qM6E/view?usp=sharing";

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
        
            videoPlayer.url = videoUrl;
            videoPlayer.Play(); // Lance la vidéo
        }
        else
        {
            // Si ce n'est pas WebGL, on charge un VideoClip local
            videoPlayer.clip = videoClip;
            videoPlayer.Play(); // Lance la vidéo
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
