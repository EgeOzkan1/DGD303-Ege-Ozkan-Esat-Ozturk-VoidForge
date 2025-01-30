using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    [Header("Experience & Leveling")]
    public int currentExperience = 0;
    public int experienceToLevelUp = 100;
    public int currentLevel = 1;

    [Header("UI Elements")]
    public TextMeshProUGUI levelText; // Text to display current level
    public TextMeshProUGUI experienceText; // Text to display experience

    private PlayerUpgrades playerUpgrades; // Reference to the player's upgrades system
    private PlayerHealth playerHealth; // Reference to the player's health system

    private void Start()
    {
        // Get references to the player scripts
        playerUpgrades = GetComponent<PlayerUpgrades>();
        playerHealth = GetComponent<PlayerHealth>();

        // Update the UI display
        UpdateUI();
    }

    // Call this method when a zombie is killed to add experience
    public void AddExperience(int amount)
    {
        currentExperience += amount;

        // Check if the player has leveled up
        if (currentExperience >= experienceToLevelUp)
        {
            LevelUp();
        }

        UpdateUI();
    }

    // Handle leveling up and automatically applying upgrades
    private void LevelUp()
    {
        currentLevel++;  // Increase level
        currentExperience = 0;  // Reset experience for the next level
        experienceToLevelUp += 50;  // Increase experience required for next level (scaling difficulty)

        // Automatically apply upgrades after leveling up
        playerUpgrades.ApplyAllUpgrades();  // Call ApplyAllUpgrades method in PlayerUpgrades
        UpdateUI();
    }

    // Update the UI with the current level and experience
    private void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }

        if (experienceText != null)
        {
            experienceText.text = "XP: " + currentExperience + "/" + experienceToLevelUp;
        }
    }
}
