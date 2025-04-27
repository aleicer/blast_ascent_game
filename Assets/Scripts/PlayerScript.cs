using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    [Header("Animation")]
    private Animator animator;

    [Header("Horizontal")]
    [SerializeField] private float movementSpeed;
    [Range(0, 0.3f)][SerializeField] private float motionSmoothig;
    private float horizontalMovement = 0f;
    private Rigidbody2D rigidBody2d;
    private Vector3 speed = Vector3.zero;
    private bool lookingRight = true;
    private InputMovement inputActions;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whichIsFloor;
    [SerializeField] private Transform floorController;
    [SerializeField] private Vector3 boxDimensions;
    [SerializeField] private bool inFloor;
    private bool jump = false;

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

    public void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        inputActions.Movement.Jump.performed += context => Jump(context);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log(horizontalMovement != 0.0f);
        horizontalMovement = inputActions.Movement.Horizontal.ReadValue<float>() * movementSpeed;
        animator.SetBool("running", horizontalMovement != 0.0f && inFloor);
    }

    private void FixedUpdate()
    {
        inFloor = Physics2D.OverlapBox(floorController.position, boxDimensions, 0f, whichIsFloor);
        Move(horizontalMovement * Time.fixedDeltaTime, jump);
        jump = false;
        animator.SetBool("jumping", jump);
    }

    private void Move(float move, bool isJump)
    {
        Vector3 targetSpeed = new Vector2(move, rigidBody2d.linearVelocity.y);
        rigidBody2d.linearVelocity = Vector3.SmoothDamp(rigidBody2d.linearVelocity, targetSpeed, ref speed, motionSmoothig);

        if (move > 0 && !lookingRight) Spin();
        else if (move < 0 && lookingRight) Spin();

        if (inFloor && isJump) 
        { 
            inFloor = false;
            rigidBody2d.AddForce(new Vector2(0f, jumpForce));
            animator.SetBool("running", false);
        }
    }

    private void Jump (InputAction.CallbackContext context)
    {
        jump = true;
        animator.SetBool("jumping", jump);
    }

    private void Spin()
    {
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(floorController.position, boxDimensions);
    }
}
