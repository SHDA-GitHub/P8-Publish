using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public static GameObject instance;

    [Header("Movement Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpMultiplier = 5f;
    private float originalSpeed;
    public bool onGround = true;
    private Vector3 movement;
    private Vector2 aim;
    private Rigidbody rb;
    [SerializeField] private bool weaponToggle = true;
    // true = gun
    // false = slash

    [Header("Player Shoot")]
    [SerializeField] float fireRate = 0f;
    private float nextFireTime = 0f;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool isShooting = false;

    [Header("Player Slash")]
    [SerializeField] private GameObject slashCollider;
    [SerializeField] private float slashDuration = 1f;
    [SerializeField] private bool slashActive = false;

    private InputSystem_Actions controls;

    private void Awake()
    {
        weaponToggle = true;
        Cursor.lockState = CursorLockMode.Locked; 
        instance = this.gameObject;

        controls = new InputSystem_Actions();
        controls.Player.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMoveCancel;
        controls.Player.Sprint.performed += OnSprint;
        controls.Player.Sprint.canceled += OnSprintCancel;
        controls.Player.Shoot.performed += OnAttack;
        controls.Player.Shoot.canceled += OnAttackCancel;
        controls.Player.Jump.performed += OnJump;
        controls.Player.Toggle.performed += OnToggleWeapon;

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

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (weaponToggle)
        {
            isShooting = true;
        }
        else
        {
            StartCoroutine(Slash());
        }
    }

    private void OnAttackCancel(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    private void OnToggleWeapon(InputAction.CallbackContext context)
    {
        Vector2 scroll = context.ReadValue<Vector2>();
        if (Mathf.Abs(scroll.y) > 0.01f)
        {
            weaponToggle = !weaponToggle;
        }
    }

    private void Shooting()
    {
        Vector3 direction = firePoint.forward;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().direction = direction;
    }

    private IEnumerator Slash()
    {
        if (slashActive)
            yield break;

        slashActive = true;

        slashCollider.SetActive(true);

        yield return new WaitForSeconds(slashDuration);

        slashCollider.SetActive(false);

        slashActive = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        float jumpForce = rb.mass * jumpMultiplier;

        if (onGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    void FixedUpdate()
    {
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

        Vector3 lookDirection = playerCamera.forward;
        lookDirection.y = 0f;
        lookDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));

        if (weaponToggle && isShooting)
        {
            if (Time.time >= nextFireTime)
            {
                Shooting();

                nextFireTime = Time.time + fireRate;
            }
        }

        if (weaponToggle == true)
        {
            gun.gameObject.SetActive(true);
        }
        else
        {
            gun.gameObject.SetActive(false);
        }
    }
}