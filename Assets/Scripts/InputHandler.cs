using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector2 InputVector { get; private set; }
    public Vector3 MousePosition { get; private set; }
    public bool IsShooting { get; private set; }

    void Update()
    {
        // Capture movement input
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        InputVector = new Vector2(h, v);

        // Capture mouse position
        MousePosition = Input.mousePosition;

        // Capture shooting input (continuous while holding)
        IsShooting = Input.GetMouseButton(0); // Holding left mouse button
    }
}
