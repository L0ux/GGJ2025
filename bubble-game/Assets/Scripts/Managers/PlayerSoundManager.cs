using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSourceEffects;
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
            if(Input.GetKeyUp(keyLeft) || Input.GetKeyUp(keyRight) || Input.GetKeyUp(keyUp) || Input.GetKeyUp(keyDown)){
                audioSourceEffects.loop = true;
                PlaySound(slide);
            } else {
                StopSound();
            }

            if(Input.GetKeyUp(keyCharge)){
                PlaySound(charge);
            }
            // HandleSound(slide, keyUp, true);
            // HandleSound(slide, keyDown, true);
            // HandleSound(slide, keyLeft, true);
            // HandleSound(slide, keyRight, true);

            // Handle charging sound
            //HandleSound(charge, keyCharge, false);

            // Play jump sound when the charge key is released
            if (Input.GetKeyUp(keyCharge))
            {
                PlaySound(jump);
            }
        }
        if (isCollidingWall && !isCollidingSoap)
        {
            // Play pop sound only if not already playing
            if (audioSourceEffects.clip != pop)
            {
                PlaySound(pop);
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

    void HandleSound(AudioClip clip, KeyCode key, bool loop)
    {
        // Play sound if key is pressed
        if (Input.GetKeyUp(key))
        {
            if (!audioSourceEffects.isPlaying || audioSourceEffects.clip != clip)
            {
                audioSourceEffects.clip = clip;
                audioSourceEffects.loop = loop;
                audioSourceEffects.Play();
            }
        } else {
            if(audioSourceEffects.isPlaying && audioSourceEffects.clip == clip){
                audioSourceEffects.Stop();
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
        if(!audioSource.isPlaying){
            audioSource.clip = clip;
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
