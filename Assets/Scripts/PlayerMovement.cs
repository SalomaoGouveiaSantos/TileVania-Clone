using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] private Color32 deathColor = new Color32(1, 1, 1, 1);
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;

    private Vector2 moveInput;
    private string LAYER_MASK_GROUND = "Ground";
    private string LAYER_MASK_CLIMBING = "Climbing";
    private string LAYER_MASK_ENEMIES = "Enemies";
    private string LAYER_MASK_HAZARDS = "Hazards";
    private string ANIM_ISRUNNING = "isRunning";
    private string ANIM_ISCLIMBING = "isClimbing";
    private string ANIM_DYING = "Dying";

    private Rigidbody2D playerRb;
    private CapsuleCollider2D bodyCollider2D;
    private BoxCollider2D feetCollider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float gravityScaleAtStart;
    private bool isAlive = true;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gravityScaleAtStart = playerRb.gravityScale;
    }

    private void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void OnFire(InputValue inputValue)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }
    private void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }
    private void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask(LAYER_MASK_GROUND))) return;

        if (value.isPressed)
        {
            // do stuff
            playerRb.velocity = new Vector2(0f, jumpSpeed);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRb.velocity.y);

        playerRb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
        animator.SetBool(ANIM_ISRUNNING, playerHasHorizontalSpeed);

    }

    private void ClimbLadder()
    {
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask(LAYER_MASK_CLIMBING)))
        {
            playerRb.gravityScale = gravityScaleAtStart;
            animator.SetBool(ANIM_ISCLIMBING, false);
            return;
        }
        Vector2 climbVelocity = new Vector2(playerRb.velocity.x, moveInput.y * climbSpeed);
        playerRb.velocity = climbVelocity;
        playerRb.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;
        animator.SetBool(ANIM_ISCLIMBING, playerHasVerticalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRb.velocity.x), 1f);
        }
    }

    private void Die()
    {
        if (bodyCollider2D.IsTouchingLayers(LayerMask.GetMask(LAYER_MASK_ENEMIES, LAYER_MASK_HAZARDS)))
        {
            isAlive = false;
            animator.SetTrigger(ANIM_DYING);
            spriteRenderer.color = deathColor;
            playerRb.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
