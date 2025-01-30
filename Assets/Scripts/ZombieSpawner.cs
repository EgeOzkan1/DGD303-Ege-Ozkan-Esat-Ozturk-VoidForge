using System.Collections;
using UnityEngine;
using TMPro; // Import TextMeshPro

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float initialSpawnInterval = 2f;
    [SerializeField] private int maxZombies = 50;
    [SerializeField] private TextMeshProUGUI clockText; // UI Timer

    private float spawnInterval;
    private int currentZombieCount = 0;
    private int zombieHealth = 100;
    private int zombieDamage = 10;
    private float zombieSpeed = 4f; // Static speed value
    private int elapsedTime = 0; // Starts at 0
    private bool scalingActive = true; // Difficulty scaling control

    private void Start()
    {
        if (zombiePrefab == null)
        {
            Debug.LogError("Zombie prefab is not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player reference is not assigned!");
            return;
        }

        if (clockText == null)
        {
            Debug.LogError("ClockText (UI) is not assigned!");
            return;
        }

        spawnInterval = initialSpawnInterval;

        StartCoroutine(SpawnZombies());
        StartCoroutine(GameTimer());
    }

    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (currentZombieCount < maxZombies)
            {
                SpawnZombie();
            }
        }
    }

    private void SpawnZombie()
    {
        if (player == null) return;

        float angle = Random.Range(0f, 360f);
        Vector3 spawnPosition = player.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;

        GameObject newZombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        currentZombieCount++;

        ZombieAI zombieAI = newZombie.GetComponent<ZombieAI>();
        if (zombieAI != null)
        {
            zombieAI.SetStats(zombieHealth, zombieDamage, zombieSpeed); // Set stats here
            zombieAI.OnDeath += () => currentZombieCount--;
        }
    }

    private IEnumerator GameTimer()
    {
        while (true) // Timer never stops
        {
            yield return new WaitForSeconds(1f);
            elapsedTime++; // Increase time
            UpdateClockDisplay();

            // Difficulty scaling stops after 5 minutes
            if (scalingActive && elapsedTime % 60 == 0)
            {
                int minutes = elapsedTime / 60;
                if (minutes <= 5)
                {
                    spawnInterval = Mathf.Max(spawnInterval * 0.8f, 0.5f);
                    zombieHealth += 25;
                    zombieDamage += 5; // Increase damage over time

                    Debug.Log($"Time: {minutes} min - Spawn Rate: {spawnInterval}s, Health: {zombieHealth}, Damage: {zombieDamage}");
                }
                else
                {
                    scalingActive = false; // Stop difficulty scaling
                    Debug.Log("Difficulty scaling has stopped.");
                }
            }
        }
    }

    private void UpdateClockDisplay()
    {
        int minutes = elapsedTime / 60;
        int seconds = elapsedTime % 60;
        clockText.text = $"{minutes:00}:{seconds:00}"; // Format like 00:00
    }
}
