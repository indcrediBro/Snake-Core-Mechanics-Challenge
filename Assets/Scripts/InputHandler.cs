using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private SnakeController snakeController;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Gameplay.Move.performed += OnMovePerformed;
    }

    private void OnDisable()
    {
        controls.Gameplay.Move.performed -= OnMovePerformed;
        controls.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        // Snap input to the nearest cardinal direction
        if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
        {
            // Horizontal movement
            inputVector = inputVector.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            // Vertical movement
            inputVector = inputVector.y > 0 ? Vector2.up : Vector2.down;
        }

        // Forward the snapped direction to the SnakeMovement script
        snakeController.SetInputDirection(inputVector.normalized);
    }
}
