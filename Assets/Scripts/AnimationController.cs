using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private InputHandler _input;

    private void Awake()
    {
        // Get references to necessary components
        _animator = GetComponent<Animator>();
        _input = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        // Call the method to update animation
        UpdateAnimation();
    }

    // Update the animation based on movement input
    private void UpdateAnimation()
    {
        // Check if the character is moving (i.e., input is not zero)
        bool isMoving = _input.InputVector.magnitude > 0;

        // Update the Animator parameter for running
        _animator.SetBool("IsRunning", isMoving);
    }
}
