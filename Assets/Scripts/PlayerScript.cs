using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rigidBody2d;
    private float horizontalMovement;
    private Vector3 speed = Vector3.zero;
    private InputMovement inputActions;

    private void Awake()
    {
        inputActions = new InputMovement();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        horizontalMovement = inputActions.Movement.Horizontal.ReadValue<float>() * movementSpeed;
    }

    private void FixedUpdate()
    {
        Move(horizontalMovement * Time.fixedDeltaTime);
    }

    private void Move(float move)
    {
        Vector3 targetSpeed = new Vector2(move, rigidBody2d.linearVelocity.y);
        rigidBody2d.linearVelocity = Vector3.SmoothDamp(rigidBody2d.linearVelocity, targetSpeed, ref speed, 0f);
    }
}
