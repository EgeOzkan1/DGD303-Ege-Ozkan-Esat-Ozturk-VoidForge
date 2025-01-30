using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int damage = 25; // Default damage value

    [SerializeField]
    private float lifetime = 2f; // Time before the bullet gets destroyed automatically

    private void Start()
    {
        // Automatically destroy the bullet after its lifetime
        Destroy(gameObject, lifetime);
    }

    // Method to set the bullet damage (can be called from the player or bullet spawner)
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit is a zombie
        ZombieAI zombie = collision.gameObject.GetComponent<ZombieAI>();
        if (zombie != null)
        {
            // Apply damage to the zombie
            zombie.TakeDamage(damage);
        }

        // Destroy the bullet upon collision with a zombie or other valid targets
        if (!collision.gameObject.CompareTag("Bullet")) // Assuming bullets have a "Bullet" tag
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Alternative to OnCollisionEnter for trigger colliders
        ZombieAI zombie = other.GetComponent<ZombieAI>();
        if (zombie != null)
        {
            // Apply damage to the zombie
            zombie.TakeDamage(damage);
        }

        // Destroy the bullet upon trigger collision with valid targets
        if (!other.CompareTag("Bullet")) // Assuming bullets have a "Bullet" tag
        {
            Destroy(gameObject);
        }
    }
}
