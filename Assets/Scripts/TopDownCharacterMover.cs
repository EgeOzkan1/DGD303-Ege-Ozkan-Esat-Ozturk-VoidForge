using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;
    private Animator _animator;

    [SerializeField]
    private bool RotateTowardMouse;

    [SerializeField]
    private float MovementSpeed = 5f; // Player's movement speed

    [SerializeField]
    private float RotationSpeed = 5f; // Player's rotation speed

    [SerializeField]
    private Camera Camera;

    [Header("Shooting")]
    [SerializeField]
    private GameObject ProjectilePrefab;

    [SerializeField]
    private Transform ProjectileSpawnPoint;

    [SerializeField]
    private float ProjectileSpeed = 10f;

    [SerializeField]
    private float ShootingCooldown = 0.5f; // Time between shots
    private float _lastShotTime;

    [Header("Sound Effects")]
    private ShootingSoundManager shootingSoundManager; // Reference to ShootingSoundManager

    private PlayerHealth playerHealth; // Reference to the player's health system

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>(); // Get the PlayerHealth component
        shootingSoundManager = GetComponent<ShootingSoundManager>(); // Get the ShootingSoundManager component
    }

    void Update()
    {
        // Calculate movement direction
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector); // Call MoveTowardTarget to move

        // Update animations based on movement and facing direction
        UpdateAnimations(movementVector);

        // Rotate character based on mouse position or movement direction
        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);
        }
        if (RotateTowardMouse)
        {
            RotateFromMouseVector();
        }

        // Handle shooting logic
        if (_input.IsShooting && Time.time >= _lastShotTime + ShootingCooldown)
        {
            ShootProjectile();
            PlayShootSound(); // Play the shooting sound when shooting
            _lastShotTime = Time.time;
        }
    }

    private void UpdateAnimations(Vector3 movementDirection)
    {
        bool isMoving = movementDirection.magnitude > 0;
        _animator.SetBool("IsMoving", isMoving);

        // Determine the facing direction of the character based on mouse rotation
        Vector3 forwardDirection = transform.forward;
        Vector3 rightDirection = transform.right;

        // Get the input direction relative to the character's facing direction
        float moveX = _input.InputVector.x;  // Horizontal movement (left/right)
        float moveY = _input.InputVector.y;  // Vertical movement (forward/backward)

        // Normalize input direction to avoid faster diagonal movement
        Vector3 inputDirection = (forwardDirection * moveY + rightDirection * moveX).normalized;

        // Set parameters for movement animation
        _animator.SetFloat("MoveX", inputDirection.x);  // Set movement in the horizontal direction (left/right)
        _animator.SetFloat("MoveY", inputDirection.z);  // Set movement in the vertical direction (forward/backward)

        // Handle shooting animation
        _animator.SetBool("IsShooting", _input.IsShooting);
    }

    private void RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;  // Keep character rotation on the horizontal plane
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = MovementSpeed * Time.deltaTime;

        // Adjust movement direction based on camera rotation
        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;

        // Move the character towards the target
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;

        return targetVector;  // Return the movement vector to update animations
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }

        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }

    private void ShootProjectile()
    {
        if (ProjectilePrefab == null || ProjectileSpawnPoint == null)
        {
            Debug.LogWarning("ProjectilePrefab or ProjectileSpawnPoint is not set!");
            return;
        }

        // Instantiate the projectile
        var projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);

        // Set its velocity in the forward direction of the spawn point
        var rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = ProjectileSpawnPoint.forward * ProjectileSpeed;
        }

        // Align the projectile's rotation to its forward movement direction
        projectile.transform.rotation = Quaternion.LookRotation(ProjectileSpawnPoint.forward);
    }

    // Play shooting sound
    private void PlayShootSound()
    {
        if (shootingSoundManager != null)
        {
            shootingSoundManager.PlayShootSound(); // Call PlayShootSound method from ShootingSoundManager
        }
    }

    // Update shooting cooldown
    public void UpdateShootingCooldown(float newCooldown)
    {
        ShootingCooldown = newCooldown;
        Debug.Log("Shooting cooldown updated.");
    }

    // Update bullet damage
    public void UpdateBulletDamage(float newBulletDamage)
    {
        Bullet bullet = ProjectilePrefab.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDamage((int)newBulletDamage);  // Cast the float to int
            Debug.Log("Bullet damage updated.");
        }
    }
}
