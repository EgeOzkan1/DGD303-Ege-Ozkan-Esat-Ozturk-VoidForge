using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For UI
using UnityEngine.SceneManagement; // To manage scene loading

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;  // Max health of the player
    public int currentHealth;  // Current health of the player

    [Header("UI")]
    public TextMeshProUGUI healthText;  // Reference to the TextMeshProUGUI element for health display

    [Header("Death Settings")]
    public string mainMenuSceneName = "MainMenu"; // The name of the scene to load when the player dies

    private void Start()
    {
        // Initialize current health to max health at the start
        currentHealth = maxHealth;

        // Update health UI display
        UpdateHealthUI();
    }

    // Method to increase health (will be used in upgrades)
    public void IncreaseHealth(int amount)
    {
        maxHealth += amount;  // Increase max health
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);  // Add health but don't exceed max health

        // Update the UI after changing health
        UpdateHealthUI();
    }

    // Method to apply damage to the player
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Update health UI after damage
        UpdateHealthUI();

        // Check if health is zero or less
        if (currentHealth <= 0)
        {
            Die();  // Call the Die function if health is 0 or below
        }
    }

    // Method to update the health UI (called whenever health changes)
    public void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";  // Update the health text on screen
        }
    }

    // Method to handle player death
    private void Die()
    {
        Debug.Log("Player has died!");

        // You can play a death animation here if you have one

        // Call the method to load the main menu after a short delay
        StartCoroutine(LoadMainMenu());
    }

    // Coroutine to load the main menu after a short delay
    private IEnumerator LoadMainMenu()
    {
        // Optional: Add a delay (e.g., 2 seconds) before transitioning to the main menu
        yield return new WaitForSeconds(2f);

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
