using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
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
    private string soapTag = "soap";
    private string wallTag = "wall";

    void Update()
    {
        // Play or stop sounds based on conditions
        if (isCollidingSoap)
        {
            HandleSound(slide, keyUp, true);
            HandleSound(slide, keyDown, true);
            HandleSound(slide, keyLeft, true);
            HandleSound(slide, keyRight, true);

            // Handle charging sound
            HandleSound(charge, keyCharge, false);

            // Play jump sound when the charge key is released
            if (Input.GetKeyUp(keyCharge))
            {
                PlaySound(jump);
                Debug.Log("JUMP");
            }
        }
        if (isCollidingWall && !isCollidingSoap)
        {
            // Play pop sound only if not already playing
            if (!audioSource.isPlaying || audioSource.clip != pop)
            {
                PlaySound(pop);
 /*               Debug.Log("PAF");*/
            }
        }
        else
        {
            // Stop any playing sound when not colliding
            StopSound();
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

    void HandleSound(AudioClip clip, KeyCode key, bool loop)
    {
        // Play sound if key is pressed
        if (Input.GetKey(key))
        {
            if (!audioSource.isPlaying || audioSource.clip != clip)
            {
                audioSource.clip = clip;
                audioSource.loop = loop;
                audioSource.Play();
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        // Stop current sound and play the new clip
        if (audioSource.isPlaying && audioSource.clip != clip)
        {
            audioSource.Stop();
        }
        if(!audioSource.isPlaying && audioSource.clip == clip){
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
