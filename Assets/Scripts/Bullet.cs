using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int Damage = 25; // Damage dealt by the bullet

    [SerializeField]
    private float Lifetime = 2f; // Time before the bullet gets destroyed automatically

    private void Start()
    {
        // Automatically destroy the bullet after its lifetime
        Destroy(gameObject, Lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit is a zombie
        ZombieAI zombie = collision.gameObject.GetComponent<ZombieAI>();
        if (zombie != null)
        {
            // Apply damage to the zombie
            zombie.TakeDamage(Damage);
        }

        // Destroy the bullet upon collision with any object
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Alternative to OnCollisionEnter for trigger colliders
        ZombieAI zombie = other.GetComponent<ZombieAI>();
        if (zombie != null)
        {
            // Apply damage to the zombie
            zombie.TakeDamage(Damage);
        }

        // Destroy the bullet upon trigger collision
        Destroy(gameObject);
    }
}
