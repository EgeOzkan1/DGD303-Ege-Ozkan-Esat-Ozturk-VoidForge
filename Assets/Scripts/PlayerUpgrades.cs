using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("Upgrades")]
    public int projectiles = 1;
    public float shootingCooldown = 0.40f;
    public float bulletDamage = 40f;  // Set default bullet damage
    public int maxHealth = 100;  // Max health of the player
    public int currentHealth;  // Current health of the player

    private PlayerHealth playerHealth; // Reference to PlayerHealth script
    private TopDownCharacterMover characterMover;  // Reference to TopDownCharacterMover script

    private void Start()
    {
        // Get the PlayerHealth and TopDownCharacterMover components at the start
        playerHealth = GetComponent<PlayerHealth>();
        characterMover = GetComponent<TopDownCharacterMover>(); // Ensure it's attached to the player

        // Reset to default values when starting a new game
        ResetUpgrades();

        // Initialize current health to max health at the start
        currentHealth = maxHealth;
        playerHealth.UpdateHealthUI();  // Ensure health UI is correct
    }

    // Call this method when the player levels up
    public void ApplyAllUpgrades()
    {
        // Apply all upgrades at once
        ReduceShootingCooldown();
        IncreaseBulletDamage();
        IncreaseHealth();

        // Update the TopDownCharacterMover script with new shootingCooldown and bulletDamage
        if (characterMover != null)
        {
            characterMover.UpdateShootingCooldown(shootingCooldown); // Pass the new shooting cooldown
            characterMover.UpdateBulletDamage(bulletDamage); // Pass the new bullet damage
        }

        // Log upgrade application
        Debug.Log("All upgrades applied at level up.");
    }

    // Reset to default values for a new game
    private void ResetUpgrades()
    {
        // Reset upgrades to default values
        bulletDamage = 40f; // Default bullet damage
        shootingCooldown = 0.40f; // Default shooting cooldown
        maxHealth = 100; // Default max health
        currentHealth = maxHealth; // Reset current health

        // Reset any other upgrades you might have
        Debug.Log("Player upgrades have been reset for a new game.");
    }

    // Reduce shooting cooldown
    private void ReduceShootingCooldown()
    {
        // Reduce shooting cooldown, hard cap to 0.05f
        shootingCooldown = Mathf.Max(0.05f, shootingCooldown - 0.05f); 
        Debug.Log("Shooting cooldown reduced.");
    }

    // Increase bullet damage
    private void IncreaseBulletDamage()
    {
        // Increase bullet damage
        bulletDamage += 5f;
        Debug.Log("Bullet damage increased.");
    }

    // Increase health
    private void IncreaseHealth()
    {
        // Increase max health and current health
        maxHealth += 15;
        currentHealth += 25; // Add to current health instead of replenishing

        // Apply the change to PlayerHealth script
        playerHealth.IncreaseHealth(20);  // Call IncreaseHealth from PlayerHealth script

        // Make sure the current health is capped correctly at the new max health
        currentHealth = Mathf.Min(currentHealth, maxHealth);  // Prevent going above max health
        Debug.Log("Max health and current health increased.");
    }
}
