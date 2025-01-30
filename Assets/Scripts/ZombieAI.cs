using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [SerializeField] private float DetectionRange = 15f;
    [SerializeField] private float MovementSpeed = 4f;
    [SerializeField] private float StoppingDistance = 1f;
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private int Damage = 10;
    [SerializeField] private float AttackRange = 1f;
    [SerializeField] private float AttackCooldown = 2f;

    [Header("Experience Drop")]
    [SerializeField] private GameObject experienceDropPrefab;  // The experience drop prefab to instantiate upon death
    [SerializeField] private int experienceAmount = 10;  // Amount of experience the zombie drops

    private Transform _player;
    private int _currentHealth;
    private float _attackTimer;
    private Animator _animator;
    private Vector3 _lastPosition;

    public event System.Action OnDeath;

    private void Start()
    {
        _currentHealth = MaxHealth;
        _attackTimer = AttackCooldown;

        _animator = GetComponent<Animator>();
        _lastPosition = transform.position;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with the tag 'Player' found in the scene.");
        }

        Debug.Log($"Zombie initialized with Movement Speed: {MovementSpeed}");
    }

    private void Update()
    {
        if (_player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= DetectionRange)
        {
            FollowPlayer(distanceToPlayer);

            if (distanceToPlayer <= AttackRange && _attackTimer >= AttackCooldown)
            {
                AttackPlayer();
            }
        }

        _attackTimer += Time.deltaTime;

        bool isMoving = Vector3.Distance(transform.position, _lastPosition) > 0.01f;
        _animator.SetBool("IsMoving", isMoving);
        _lastPosition = transform.position;
    }

    private void FollowPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > StoppingDistance)
        {
            Vector3 directionToPlayer = (_player.position - transform.position).normalized;
            directionToPlayer.y = 0;  // Keep movement on the XZ plane
            transform.position += directionToPlayer * MovementSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void AttackPlayer()
    {
        if (_attackTimer >= AttackCooldown)
        {
            PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Damage);
                Debug.Log($"Zombie attacked the player for {Damage} damage!");
                _attackTimer = 0f;
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on player!");
            }
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

        // Spawn an experience drop at the zombie's position
        if (experienceDropPrefab != null)
        {
            GameObject experienceDrop = Instantiate(experienceDropPrefab, transform.position, Quaternion.identity);
            ExperienceDrop experienceScript = experienceDrop.GetComponent<ExperienceDrop>();
            if (experienceScript != null)
            {
                experienceScript.Amount = experienceAmount;  // Set the experience drop amount
            }
        }

        // Trigger zombie death animation
        _animator.SetTrigger("Die");
        
        // Destroy the zombie after a short delay to allow the death animation to play
        Destroy(gameObject);
    }

    public void SetStats(int health, int damage, float speed)
    {
        MaxHealth = health;
        _currentHealth = health;
        Damage = damage;
        MovementSpeed = speed;
        Debug.Log($"Zombie stats updated: Health = {MaxHealth}, Damage = {Damage}, Speed = {MovementSpeed}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
