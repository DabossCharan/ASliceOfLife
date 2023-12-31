using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // References to Player Components Or Outside
    [SerializeField] private LayerMask layers;
    private Rigidbody2D playerRb;
    private CapsuleCollider2D playerCollider;
    private SurfaceInteractions surfaceInteractions;

    // Input
    public float horizontalInput;
    public bool jumpKeyDown;
    public bool jumpKeyUp;
    private float jumpTime; // This variable is for WHEN the jump was pressed, not the duration of the jump

    // Horizontal Movement
    public float horizontalSpeed = 14;
    public float horizontalAccel = 120;

    // Jump
    public float jumpForce = 30;
    private float smallJumpGravityModifier = 3;
    private bool smallJump = true;
    private bool canBufferJump = false;
    public bool grounded;
    public bool aboutToLand;
    public float distanceThreshold;

    // Gravity
    public float maxFallSpeed = 125f;
    public float minFallSpeed = 75f;
    public float fallSpeedLim = -70f;
    private float fallSpeed;

    // Collisions
    public bool touchingGround;
    public bool touchingCeil;
    public bool touchingRight;
    public bool touchingLeft;

    // Final Movement
    public Vector2 playerVelocity;
    public Quaternion playerRotation;

    // Sprite Appearance
    private bool flipSprite = false;

    // Animator Parameters
    public bool isMoving;
    public bool justJumped; // Use for animations
    public bool justLanded; // Use for animations

    // Continue to Modify and Understand how Tarodev's stuff works
    [SerializeField] private float _apexBonus = 2;
    [SerializeField] private float _jumpApexThreshold = 10f;
    [SerializeField] private float _jumpBuffer = 0.1f;
    private float apexPoint; // Becomes 1 at the apex of a jump
    private bool _cachedQueryStartInColliders;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>(); 
        playerCollider = GetComponent<CapsuleCollider2D>();
        surfaceInteractions = GameObject.FindGameObjectWithTag("SurfaceInteractions").GetComponent<SurfaceInteractions>();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders; // STUDY THIS
        playerRotation = transform.rotation;
    }

    void Update()
    {
        GetInput();
        Collisions();
        Walk(); // Horizontal movement
        JumpApex(); // Affects fall speed, so calculate before gravity
        Gravity(); // Vertical movement
        Jump(); // Possibly overrides vertical
        MoveCharacter(); // Actually perform the axis movement
    }

    private void Walk()
    {
        if (horizontalInput != 0)
        {
            isMoving = true;
            playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, horizontalInput * horizontalSpeed, horizontalAccel * Time.deltaTime);
            // Bonus at the apex of a jump
            var apexBonus = horizontalInput * _apexBonus * apexPoint;
            playerVelocity.x += apexBonus * Time.deltaTime;
        }
        else
        {
            if (surfaceInteractions.climbingIceCream)
            {
                playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, -surfaceInteractions.iceCreamSlide, horizontalAccel * Time.deltaTime);
            } else
            {
                playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, 0, horizontalAccel * Time.deltaTime);
            }

            if (playerVelocity.x == 0 || (surfaceInteractions.climbingIceCream))
            {
                isMoving = false;
            }
        }

        if (playerRb.velocity.x > 0 && touchingRight || playerRb.velocity.x < 0 && touchingLeft)
        {
            playerVelocity.x = 0;
        }
    }

    private void JumpApex()
    {
        // Gets stronger the closer to the top of the jump
        if (!touchingGround)
        {
            apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(playerVelocity.y));
            fallSpeed = Mathf.Lerp(minFallSpeed, maxFallSpeed, apexPoint);
        }
        else
        {
            apexPoint = 0;
        }
    }
    private void Jump()
    {
        // Handle Small Jumps
        if (!grounded && jumpKeyUp && !smallJump && playerRb.velocity.y > 0)
        {
            smallJump = true;
        }

        // Calculate if Can Buffer Jump or Quit Function Early
        canBufferJump = grounded && jumpTime + _jumpBuffer > Time.time;
        if (!canBufferJump)
        {
            return;
        }

        if (grounded || canBufferJump)
        {
            grounded = false;
            playerVelocity.y = jumpForce;
            smallJump = false;
            transform.rotation = playerRotation;


            //if (touchingRight && surfaceInteractions.climbingIceCream)
            //{
            //    playerVelocity.x = 0;
            //}
        }
    }
 
    private void Gravity()
    {
        if (!grounded)
        {
            // Function will have variable gravity depending on whether player is
            // free-falling or ended a jump early
            // Ending Jump Early Scenario
            if (smallJump && playerVelocity.y > 0)
            {
                fallSpeed *= smallJumpGravityModifier;
            }

            playerVelocity.y = Mathf.MoveTowards(playerVelocity.y, fallSpeedLim, fallSpeed * Time.deltaTime);
        } else if (grounded) //&& !surfaceInteractions.climbingIceCream)
        {
            playerVelocity.y = 0;
        }
    }

    private void Collisions()
    {
        Physics2D.queriesStartInColliders = false;
        touchingGround = Physics2D.CapsuleCast(playerCollider.bounds.center, playerCollider.size, playerCollider.direction, 0, -transform.up, 0.1f, layers);
        touchingCeil = Physics2D.CapsuleCast(playerCollider.bounds.center, playerCollider.size, playerCollider.direction, 0, Vector2.up, 0.05f, layers);
        touchingRight = Physics2D.CapsuleCast(playerCollider.bounds.center, playerCollider.size, playerCollider.direction, 0, transform.right, 0.05f, layers);
        touchingLeft = Physics2D.CapsuleCast(playerCollider.bounds.center, playerCollider.size, playerCollider.direction, 0, -transform.right, 0.05f, layers);
        aboutToLand = !touchingGround && Physics2D.CapsuleCast(playerCollider.bounds.center, playerCollider.size, playerCollider.direction, 0, -transform.up, distanceThreshold, layers);

        if (touchingCeil)
        {
            playerVelocity.y = 0;
        }

        if (touchingGround && !grounded)
        {
            grounded = true;
            canBufferJump = true;
            smallJump = false;
            justLanded = true;
        } else if (!touchingGround && grounded)
        {
            grounded = false;
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders; // STUDY THIS
    }
    private void GetInput()
    {
        jumpKeyDown = Input.GetButtonDown("Jump");
        jumpKeyUp = Input.GetButtonUp("Jump");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (jumpKeyDown)
        {
            jumpTime = Time.time;
            justJumped = true;
            justLanded = false;

            // Deal With Surface Interactions
            surfaceInteractions.stick = false;
            surfaceInteractions.climbingIceCream = false;
        } else
        {
            justJumped = false;
            justLanded = false;
        }

        // Change Sprite Direction Based on Input
        if (horizontalInput > 0 && flipSprite)
        {
            FlipSprite();
        }
        else if (horizontalInput < 0 && !flipSprite)
        {
            FlipSprite();
        }
    }
    private void MoveCharacter()
    {
        playerRb.velocity = playerVelocity;
    }
    private void FlipSprite()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        flipSprite = !flipSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OrangePlatform"))
        {
            surfaceInteractions.stick = true;
            surfaceInteractions.orangeSurfaceActive = true;
            surfaceInteractions.iceCreamSurfaceActive = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("OrangePlatform"))
        {
            surfaceInteractions.stick = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("IceCream"))
        {
            ContactPoint2D contactPoint = collision.GetContact(0);
            Quaternion contactPointRotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
            transform.rotation = contactPointRotation;
            //transform.rotation = collision.collider.transform.rotation;
            surfaceInteractions.climbingIceCream = true;
            surfaceInteractions.iceCreamSurfaceActive = true;
            surfaceInteractions.orangeSurfaceActive = false;
        } else if (collision.collider.CompareTag("Slope"))
        {
            transform.rotation = collision.transform.rotation;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("IceCream"))
        {
            ContactPoint2D contactPoint = collision.GetContact(0);
            Quaternion contactPointRotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
            transform.rotation = contactPointRotation;
            surfaceInteractions.climbingIceCream = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Slope") || collision.collider.CompareTag("IceCream"))
        {
            transform.rotation = playerRotation;
        }
    }
}

