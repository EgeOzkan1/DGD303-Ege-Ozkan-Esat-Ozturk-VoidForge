using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private float DetectionRange = 10f; // Distance at which the zombie detects the player

    [SerializeField]
    private float MovementSpeed = 2f; // Zombie movement speed

    [SerializeField]
    private float StoppingDistance = 1f; // Distance at which the zombie stops near the player

    [SerializeField]
    private int MaxHealth = 100; // Maximum health of the zombie

    private Transform _player; // Cached reference to the player
    private int _currentHealth; // Current health of the zombie

    private void Start()
    {
        _currentHealth = MaxHealth;

        // Automatically find the player by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with the tag 'Player' found in the scene. Zombies cannot follow the player.");
        }
    }

    private void Update()
    {
        if (_player == null) return;

        // Only follow the player if they are within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer <= DetectionRange)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // Check if the zombie is within the stopping distance
        if (distanceToPlayer > StoppingDistance)
        {
            // Move toward the player
            Vector3 directionToPlayer = (_player.position - transform.position).normalized;
            directionToPlayer.y = 0; // Keep movement on the horizontal plane

            transform.position += directionToPlayer * MovementSpeed * Time.deltaTime;

            // Rotate to face the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log($"Zombie took {damage} damage. Remaining health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Zombie died!");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);
    }
}
