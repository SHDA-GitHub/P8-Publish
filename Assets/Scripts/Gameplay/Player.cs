using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float jumpMultiplier = 5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravityMultiplier = 2.5f;
    [SerializeField] private float fallMultiplier = 3.5f;
    private float originalSpeed;
    public bool onGround = true;
    private Vector3 movement;
    private Vector2 aim;
    private Rigidbody rb;

    [Header("Player groundcheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMoveCancel;
        controls.Player.Sprint.performed += OnSprint;
        controls.Player.Sprint.canceled += OnSprintCancel;
        controls.Player.Jump.performed += OnJump;

        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        movement = new Vector3(input.x, 0, input.y);
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        movement = Vector3.zero;
    }

    public void OnLookRotation(InputAction.CallbackContext context)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
            speed = originalSpeed * 1.5f;
    }

    public void OnSprintCancel(InputAction.CallbackContext context)
    {
            speed = originalSpeed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (onGround)
        {
            rb.AddForce(Vector3.up * rb.mass * jumpMultiplier, ForceMode.Impulse);
            onGround = false;
        }
    }

    void FixedUpdate()
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (movement != Vector3.zero)
        {
            Vector3 camForward = playerCamera.forward;
            Vector3 camRight = playerCamera.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * movement.z) + (camRight * movement.x);
            moveDirection.Normalize();

            Vector3 moveOffset = moveDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveOffset);
        }


        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}