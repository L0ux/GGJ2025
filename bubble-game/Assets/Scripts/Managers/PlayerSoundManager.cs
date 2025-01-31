using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSourceEffects;
    public AudioSource audioSourceMovement;
    public AudioClip pop;
    public AudioClip slide;
    public AudioClip charge;
    public AudioClip jump;

    public KeyCode keyUp = KeyCode.UpArrow;
    public KeyCode keyDown = KeyCode.DownArrow;
    public KeyCode keyLeft = KeyCode.LeftArrow;
    public KeyCode keyRight = KeyCode.RightArrow;
    public KeyCode keyCharge = KeyCode.Space;

    private bool isCollidingSoap = false;
    private bool isCollidingWall = false;

    private string soapTag = "bordCollant";
    private string wallTag = "wall";

    void Update()
    {
        // Play or stop sounds based on conditions
        if (isCollidingSoap)
        {
            if(Input.GetKey(keyLeft) || Input.GetKey(keyRight) || Input.GetKey(keyUp) || Input.GetKey(keyDown)){
                audioSourceMovement.loop = true;
                PlaySound(slide, audioSourceMovement);
            } else {
                StopSound(audioSourceMovement);
            }

            if(Input.GetKey(keyCharge)){
                PlaySound(charge, audioSourceEffects);
            }

            // Play jump sound when the charge key is released
            if (Input.GetKeyUp(keyCharge))
            {
                PlaySound(jump, audioSourceEffects);
            }
        }
        if (isCollidingWall && !isCollidingSoap)
        {
            // Play pop sound only if not already playing
            if (audioSourceEffects.clip != pop)
            {
                PlaySound(pop, audioSourceEffects);
                Debug.Log("PAF");
                isCollidingWall = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check collision with soap
        if (collision.gameObject.CompareTag(soapTag))
        {
            isCollidingSoap = true;
            isCollidingWall = false;
        }
        // Check collision with wall
        else if (collision.gameObject.CompareTag(wallTag))
        {
            isCollidingWall = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Reset flags when leaving the collision
        if (collision.gameObject.CompareTag(soapTag))
        {
            isCollidingSoap = false;
        }
        if (collision.gameObject.CompareTag(wallTag))
        {
            isCollidingWall = false;
        }
    }

    void PlaySound(AudioClip clip, AudioSource src)
    {
        // Stop current sound and play the new clip
        if (src.isPlaying && src.clip != clip)
        {
            src.Stop();
        }
        if(!src.isPlaying){
            src.clip = clip;
            src.loop = false;
            src.Play();
        }
    }

    void StopSound(AudioSource src)
    {
        if (src.isPlaying)
        {
            src.Stop();
        }
    }
}
