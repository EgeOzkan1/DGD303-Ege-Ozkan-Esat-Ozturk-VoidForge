using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the target position
            Vector3 targetPosition = player.position + offset;

            // Smoothly interpolate to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}
