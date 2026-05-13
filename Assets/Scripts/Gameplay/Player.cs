using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float speed = 5f;
    private float originalSpeed;
    public bool onGround = true;
    private Vector3 movement;
    private Vector2 aim;
    private Rigidbody rb;

    [Header("Player groundcheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Player Shoot")]
    [SerializeField] float fireRate = 0f;
    private float nextFireTime = 0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool isShooting = false;

    [Header("Player Slash")]
    [SerializeField] private GameObject slashCollider;
    [SerializeField] private float slashCooldown = 1f;
    [SerializeField] private bool slashActive = false;

    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMoveCancel;
        controls.Player.Sprint.performed += OnSprint;
        controls.Player.Sprint.canceled += OnSprintCancel;
        controls.Player.Shoot.performed += OnShoot;
        controls.Player.Shoot.canceled += OnShootCancel;
        controls.Player.Slash.performed += OnSlash;
        //controls.Player.Jump.performed += OnJump;

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

    private void OnShoot(InputAction.CallbackContext context)
    {
        isShooting = true;
    }

    private void OnShootCancel(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    private void Shooting()
    {
        Vector3 direction = firePoint.forward;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().direction = direction;
    }

    private void OnSlash(InputAction.CallbackContext context)
    {
        StartCoroutine(Slash());
    }

    IEnumerator Slash()
    {
        if (slashActive == false)
        {
            slashCollider.SetActive(true);
            slashActive = true;
            yield return new WaitForSeconds(slashCooldown);
            slashCollider.SetActive(false);
            slashActive = false;
        }
    }

    private bool AbleToShoot()
    {
        return isShooting;
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

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }

        if (Time.time >= nextFireTime && AbleToShoot())
        {
            Shooting();
            nextFireTime = Time.time + fireRate;
        }
    }
}