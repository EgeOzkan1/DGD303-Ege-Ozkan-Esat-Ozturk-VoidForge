using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceDrop : MonoBehaviour
{
    public int Amount = 90;  // Amount of experience this drop gives to the player

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with the experience drop
        if (other.CompareTag("Player"))
        {
            // Find the PlayerExperience component and add the experience
            PlayerExperience playerExperience = other.GetComponent<PlayerExperience>();
            if (playerExperience != null)
            {
                playerExperience.AddExperience(Amount);  // Add the experience to the player
                Debug.Log($"Player collected {Amount} experience!");
            }

            // Destroy the experience drop after it's collected
            Destroy(gameObject);
        }
    }
}
