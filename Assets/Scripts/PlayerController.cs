using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Character Type")]
    [Tooltip("Tick vào đây nếu nhân vật biết bay (Ví dụ: Ajaw)")]
    public bool isFlying = false;

    [Header("Movement Settings")]
    private float moveSpeed = 5f;
    [Tooltip("Lực nhảy (Chỉ có tác dụng với nhân vật đi bộ như Dodoco)")]
    private float jumpForce = 7.5f;
    public bool hasJumpAnimation = true;
    public bool invertFlip = false;

    [Header("Sound Effects (SFX)")]
    public AudioSource audioSource; // Nơi phát ra âm thanh
    public AudioClip moveSFX;       // Tiếng bay (Ajaw)
    public AudioClip jumpSFX;       // Tiếng nhảy (Dodoco)

    [HideInInspector]
    public bool isControlled = false;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (isFlying)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 1f; 
        }
    }

    void Update()
    {
        if (!isControlled)
        {
            rb.linearVelocity = isFlying ? Vector2.zero : new Vector2(0, rb.linearVelocity.y);

            if (isFlying && audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            return;
        }

        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;

            if (isFlying)
            {
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY = 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY = -1f;
            }
        }

        if (isFlying)
        {
            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);

            if (audioSource != null && moveSFX != null)
            {
                bool isMoving = moveX != 0 || moveY != 0;
                if (isMoving)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = moveSFX;
                        audioSource.Play();
                    }
                }
                else
                {
                    if (audioSource.isPlaying && audioSource.clip == moveSFX)
                    {
                        audioSource.Stop();
                    }
                }
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                if (audioSource != null && jumpSFX != null)
                {
                    audioSource.PlayOneShot(jumpSFX);
                }
            }
        }

        if (moveX != 0)
        {
            float currentSizeX = Mathf.Abs(transform.localScale.x);
            float currentSizeY = transform.localScale.y;
            float currentSizeZ = transform.localScale.z;

            float flipDirection = invertFlip ? -1f : 1f;
            transform.localScale = new Vector3(Mathf.Sign(moveX) * currentSizeX * flipDirection, currentSizeY, currentSizeZ);
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.01f || (isFlying && Mathf.Abs(rb.linearVelocity.y) > 0.01f);
        animator.SetBool("IsRunning", isMoving);
        if (!isFlying && hasJumpAnimation)
        {
            bool isJumping = rb.linearVelocity.y > 0.01f;
            animator.SetBool("IsJumping", isJumping);
        }
    }
}