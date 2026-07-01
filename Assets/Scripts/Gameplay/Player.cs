using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public static GameObject instance;

    [Header("Movement Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject playerRig;
    [SerializeField] private Animator animator;
    public float jumpMultiplier = 5f;
    public float originalSpeed;
    public float speed = 5f;
    public bool onGround = true;
    private Vector3 movement;
    private Vector2 aim;
    private Rigidbody rb;

    [Header("Weapon Toggle")]
    [SerializeField] private float toggleCooldown = 0.25f;
    [SerializeField] private bool weaponToggle = true;
    [SerializeField] private Image toggleIcon;
    [SerializeField] private Sprite gunSprite;
    [SerializeField] private Sprite meleeSprite;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource walkAudio;
    [SerializeField] private TextMeshProUGUI bulletCounter;
    private float nextToggleTime = 0f;
    // true = gun
    // false = slash

    [Header("Player Shoot")]
    [SerializeField] private float fireRate = 0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    public bool isShooting = false;
    private float nextFireTime = 0f;
    public float gunDMG = 7f;
    public float gunSpeed = 10f;

    [Header("Player Slash")]
    [SerializeField] private GameObject slashCollider;
    public bool slashActive = false;
    public float knockbackStrength = 1f;
    public float slashDuration = 1f;
    public float slashHitbox = 1f;
    public float meleeDMG = 12f;

    [Header("Gun Jam Settings")]
    public int bulletsBeforeJam = 35;
    [SerializeField] private float jamDuration = 3f;
    private int bulletsShot = 0;
    private bool isJammed = false;

    [Header("Shot Reset Settings")]
    [SerializeField] private float resetShotTime = 2f;
    private float lastShotTime;
    private InputSystem_Actions controls;

    [Header("Critical Hit Settings")]
    public float critChance = 1f;
    public float critEffect = 1.5f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioClip cockingGun;
    [SerializeField] private AudioClip meleeSlash;
    [SerializeField] private AudioClip switchToGun;
    [SerializeField] private AudioClip switchToMelee;
    [SerializeField] private AudioClip equip;
    [SerializeField] private AudioClip walk;
    [SerializeField] private AudioClip hurt;

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
        controls.Player.Toggle.performed += OnToggleWeapon;

        rb = GetComponent<Rigidbody>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (animator == null)
        {
            animator = playerRig.GetComponent<Animator>();
        }

        originalSpeed = speed;

        MeleeCollision melee = slashCollider.GetComponent<MeleeCollision>();

        if (melee != null)
        {
            melee.damageToDeal = (ushort)meleeDMG;
            melee.knockbackStrength = knockbackStrength;
        }

        slashCollider.transform.localScale =
            Vector3.one * slashHitbox;

        UpdateBulletCounter();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        animator.SetBool("IsWalking", true);
        walkAudio.clip = walk;
        walkAudio.loop = true;
        walkAudio.Play();
        Vector2 input = context.ReadValue<Vector2>();
        movement = new Vector3(input.x, 0, input.y);
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        animator.SetBool("IsWalking", false);
        walkAudio.Stop();
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
            animator.SetBool("IsShooting", true);
        }
        else
        {
            StartCoroutine(Slash());
        }
    }

    private void OnAttackCancel(InputAction.CallbackContext context)
    {
        animator.SetBool("IsShooting", false);
        isShooting = false;
    }

    private void OnToggleWeapon(InputAction.CallbackContext context)
    {
        if (Time.time < nextToggleTime)
            return;

        Vector2 scroll = context.ReadValue<Vector2>();

        if (Mathf.Abs(scroll.y) > 0.01f)
        {
            weaponToggle = !weaponToggle;
            nextToggleTime = Time.time + toggleCooldown;

            toggleIcon.sprite = weaponToggle ? gunSprite : meleeSprite;
            animator.SetBool("HasGun", weaponToggle);

            audioSource.PlayOneShot(weaponToggle ? switchToGun : switchToMelee);
        }
    }

    private void Shooting()
    {
        if (isJammed)
            return;

        audioSource.PlayOneShot(shoot);

        Vector3 direction = firePoint.forward;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        int roll = Random.Range(0, 100);

        if (roll <= critChance)
        {
            bulletScript.DamageToDeal = Mathf.RoundToInt(gunDMG * critEffect);
        }
        else
        {
            bulletScript.DamageToDeal = Mathf.RoundToInt(gunDMG);
        }

        bulletScript.direction = direction;
        bulletScript.speed = Mathf.RoundToInt(gunSpeed);

        bulletsShot++;

        UpdateBulletCounter();

        lastShotTime = Time.time;

        if (bulletsShot >= bulletsBeforeJam)
        {
            StartCoroutine(JamGun());
        }
    }

    private IEnumerator JamGun()
    {
        isJammed = true;

        Debug.Log("Gun Jammed!");

        audioSource.PlayOneShot(reload);

        yield return new WaitForSeconds(jamDuration);

        bulletsShot = 0;
        UpdateBulletCounter();
        isJammed = false;

        Debug.Log("Gun Unjammed!");

        audioSource.PlayOneShot(cockingGun);
    }

    private void UpdateBulletCounter()
    {
        int currentBullets = bulletsBeforeJam - bulletsShot;
        bulletCounter.text = $"Bullets: {currentBullets:00}/{bulletsBeforeJam:00}";
    }

    private IEnumerator Slash()
    {
        animator.SetTrigger("Stab");

        audioSource.PlayOneShot(meleeSlash);

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
        animator.SetTrigger("Jump");
        if (!context.performed) return;

        float jumpForce = rb.mass * jumpMultiplier;

        if (onGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }
    }

    public void UpdateStats()
    {
        MeleeCollision melee = slashCollider.GetComponent<MeleeCollision>();
        if (melee != null)
        {
            melee.damageToDeal = (ushort)meleeDMG;
            melee.knockbackStrength = knockbackStrength;
        }
        slashCollider.transform.localScale =
            Vector3.one * slashHitbox;
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
       toggleIcon.sprite = gunSprite;
       animator.SetBool("HasGun", true);
    }
    else
    {
        toggleIcon.sprite = meleeSprite;
        animator.SetBool("HasGun", false);
    }

        MeleeCollision melee = slashCollider.GetComponent<MeleeCollision>();

        if (melee != null)
        {
            melee.damageToDeal = (ushort)meleeDMG;
            melee.knockbackStrength = knockbackStrength;
        }

        if (!isJammed &&
            bulletsShot > 0 &&
            Time.time >= lastShotTime + resetShotTime)
        {
            bulletsShot = 0;
            UpdateBulletCounter();

            Debug.Log("Shot counter reset.");
        }
    }
}