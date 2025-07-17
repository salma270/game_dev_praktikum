using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
    [Header("Health System")]
    public int maxHealth = 3;
    private int currentHealth;
    public TextMeshProUGUI healthText;
    public HeartUI heartUI;  // drag dari inspector
    public GameObject gameOverPanel;


    [Header("Knockback Settings")]
    [SerializeField] private float knockBackTime = 0.2f;
    [SerializeField] private float knockBackThrust = 10f;

    private bool isKnockedBack = false;

    [Header("Respawn Settings")]
    public Transform respawnPoint;  // titik aman untuk respawn
    public float fallThresholdY = -10f;  // Y batas bawah sebelum dianggap jatuh
    private bool isDeadByFall = false;

    [Header("Ground Detection")]
    public LayerMask groundLayer; // Pilih layer Ground di Inspector

    public void SetRespawnPoint(Vector3 newRespawnPosition)
    {
        Collider2D col = GetComponent<Collider2D>();
        float offsetY = col.bounds.extents.y + 0.05f;

        Vector2 origin = new Vector2(newRespawnPosition.x, newRespawnPosition.y + 2f);
        float maxDistance = 10f;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, maxDistance, groundLayer);

        if (hit.collider != null)
        {
            float safeY = hit.point.y + offsetY;
            Vector3 safePos = new Vector3(hit.point.x, safeY, transform.position.z);

            respawnPoint.position = safePos;
            Debug.Log("SAFE respawn at: " + safePos);
        }
        else
        {
            Vector3 fallback = newRespawnPosition + new Vector3(0f, offsetY, 0f);
            respawnPoint.position = fallback;
            Debug.LogWarning("Ground not found. Using fallback: " + fallback);
        }
    }


    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController; // tambahkan PlayerInputActions

    // Untuk input dari button UI
    private float mobileInputX = 0f;

    private Vector2 moveInput;
    private bool isJumping = false;

    private enum MovementState { idle, walk, jump, fall, run }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
        UpdateHealthUI(); // tampilkan awal


        playerController = new PlayerController(); //Inisialisasi PlayerInputActions
    }

    private void OnEnable()
    {
        playerController.Enable();

        playerController.movement.move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerController.movement.move.canceled += ctx => moveInput = Vector2.zero;

        playerController.movement.jump.performed += ctx => Jump();


    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    private void Update()
    {
        if (!isDeadByFall && transform.position.y < fallThresholdY)
        {
            isDeadByFall = true;
            TakeDamage(1, Vector2.zero);  // zero arah knockback

            if (currentHealth > 0) // hanya respawn kalau belum game over
                StartCoroutine(RespawnAfterFall());
        }

        // Jika menggunakan mobile input, pakai itu
        if (Application.isMobilePlatform)
        {
            moveInput = new Vector2(mobileInputX, 0f);
        }
        else
        {
            // Kalau bukan mobile, pakai Input System
            moveInput = playerController.movement.move.ReadValue<Vector2>();
        }

    }

    private void FixedUpdate()
    {
        //gabungan mobile
        Vector2 targetVelocity = new Vector2((moveInput.x + mobileInputX) * moveSpeed, rb.velocity.y);
        rb.velocity = targetVelocity;

        UpdateAnimation();

        // Reset isJumping hanya saat grounded dan velocity Y mendekati 0
        if (isGrounded() && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            isJumping = false;
        }

    }

    private void UpdateAnimation()
    {
        MovementState state;

        // Gabungkan input dari keyboard dan mobile
        float horizontal = moveInput.x != 0 ? moveInput.x : mobileInputX;

        // Cek arah jalan
        if (horizontal > 0f)
        {
            state = MovementState.walk;
            sprite.flipX = false;
        }
        else if (horizontal < 0f)
        {
            state = MovementState.walk;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        // Cek apakah sedang lompat atau jatuh
        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
        }

        anim.SetInteger("state", (int)state);
    }


    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Jump()
    {
        // Cek ulang grounded saat ini, dan jangan gunakan isJumping (karena bisa delay)
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
        }
    }

    // Fungsi ini dipanggil saat tombol kanan ditekan
    public void MoveRight(bool isPressed)
    {
        if (isPressed)
            mobileInputX = 1f;
        else if (mobileInputX == 1f)
            mobileInputX = 0f;
    }

    public void MoveLeft(bool isPressed)
    {
        if (isPressed)
            mobileInputX = -1f;
        else if (mobileInputX == -1f)
            mobileInputX = 0f;
    }

    // Fungsi ini dipanggil saat tombol lompat ditekan
    public void MobileJump()
    {
        if (isGrounded())
        {
            Jump();
        }
    }
    
    public void TakeDamage(int damage, Vector2 direction)
    {
        if (isKnockedBack || currentHealth <= 0) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Game Over!");
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true); // tampilkan UI Game Over
            
            Time.timeScale = 0; // pause game
        }

        StartCoroutine(HandleKnockback(direction.normalized));
        UpdateHealthUI();
    }


private void UpdateHealthUI()
    {
        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth);
    }



    private IEnumerator HandleKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero;

        Vector2 force = direction * knockBackThrust * rb.mass;
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    private IEnumerator RespawnAfterFall()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        float offsetY = GetComponent<Collider2D>().bounds.extents.y + 0.05f;
        Vector3 safePosition = new Vector3(respawnPoint.position.x, respawnPoint.position.y + offsetY, transform.position.z);
        transform.position = safePosition;

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        rb.isKinematic = false;

        isDeadByFall = false;
    }

    
}
