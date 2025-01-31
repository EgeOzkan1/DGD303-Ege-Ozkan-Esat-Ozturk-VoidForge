using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSoundManager : MonoBehaviour
{
    [SerializeField] 
    private AudioClip shootSound;  // The sound that will play when shooting
    
    private AudioSource audioSource;  // AudioSource component to play the sound

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component
        if (audioSource == null)
        {
            // Add AudioSource if not attached
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Method to play the shoot sound
    public void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);  // Play the sound one time
        }
    }
}
