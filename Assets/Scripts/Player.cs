using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Configuration Parameters

    //death
    [Header("Death")]
    [SerializeField] GameObject deathParticles = null;

    //horizontal movement
    [Header("Horizontal Movement")]
    [SerializeField] float runSpeed = 5f;

    //climbing
    [Header("Climbing")]
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] Transform bodyPos = null;
    [SerializeField] float bodyWidth = 0.5f;
    [SerializeField] float bodyHeight = 0.5f;

    //jumping
    [Header("Jumping")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float maxJumpTime = 0.35f;
    [SerializeField] Transform feetPos = null;
    [SerializeField] float checkRadius = 0.3f;
    [SerializeField] float bounceAmount = 2f;

    //Summoning
    [Header("Summoning")]
    [SerializeField] float summonDelay = 2f;
    [SerializeField] Transform rightDashPos = null;
    [SerializeField] Transform leftDashPos = null;
    [SerializeField] GameObject birdExplosion = null;

    //SFX
    [Header("SFX")]
    [SerializeField] AudioClip summonSound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] AudioClip jumpSound = null;

    //cache
    SpriteRenderer sr;
    Animator animator;
    Rigidbody2D rb;
    Vector2 velocity;
    Collider2D playerCollider;

    //input variables
    float horizontalMoveInput;
    float verticalMoveInput;

    //jump variables
    float jumpTimeCounter;
    bool isGrounded;
    bool isJumping;
    bool isOnBouncer;

    //climbing variables
    float gravity;
    bool touchingLadder;
    Vector2 bodySize;

    //summoning variables
    float summonTimer;
    bool canDash;
    bool summoning;
    Vector2 dashPos;
    Vector2 dashDirection;


    //death variables
    bool isAlive;

    //-----------------------------------------------------------------START - UPDATE - FIXED UPDATE----------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        gravity = rb.gravityScale;
        bodySize = new Vector2(bodyWidth, bodyHeight);
        isAlive = true;
        summonTimer = 0;
        canDash = false;
        summoning = false;
        dashPos = rightDashPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (!summoning)
            {
                Jump();
                Run();
                Climb();
                Bounce();

                if (GameManager.Instance.SummonAbility())
                {
                    SummonBird();
                }
            }
        }
    }

    //Update the velocity based on the physics engine.
    private void FixedUpdate()
    {
        if (summoning)
        {
            //keeps the player in positon will summoning
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            //set velocity of x to speed and direction, keep y velocity the same
            rb.velocity = new Vector2(horizontalMoveInput * runSpeed, rb.velocity.y);
        }
    }


    //-----------------------------------------------------------------Player Functions----------------------------------------------

    //Function for player death
    private void Die()
    {
        isAlive = false;
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        MusicManager.Instance.RandomiseSfx(deathSound);
        GameManager.Instance.PlayerRespawn();
        Destroy(this.gameObject);
    }

    //If player collides with the hazard layer (14) then call Die() function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            Die();
        }
    }

    //Function for player running
    private void Run()
    {
        //get horizontal input
        horizontalMoveInput = Input.GetAxis("Horizontal");

        //if horizontal input is active
        if (horizontalMoveInput != 0)
        {

            //trigger running animation
            animator.SetBool("Running", true);

            if (isGrounded)
            {
                //Flip Sprite
                if (horizontalMoveInput < 0)
                {
                    //flip left
                    sr.flipX = true;
                }
                else
                {
                    //flip right
                    sr.flipX = false;
                }
            }

        }
        else
        {
            //transition back to idle animation
            animator.SetBool("Running", false);
        }
    }

    private void Bounce()
    {
        //Create a collision circle to check if touching an object of the bouncing layer
        isOnBouncer = Physics2D.OverlapCircle(feetPos.position, checkRadius, LayerMask.GetMask("Bounce"));

        if (isOnBouncer)
        {
            MusicManager.Instance.RandomiseSfx(jumpSound);
            rb.velocity = Vector2.up * bounceAmount;
        }

    }

    //Function for player jumping
    private void Jump()
    {
        //Create a collision circle to check if touching the ground layer
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, LayerMask.GetMask("Ground"));

        //if grounded then set the falling animation to false
        if (isGrounded)
        {
            animator.SetBool("Falling", false);
        }

        //if grounded and pressing the jump button, set upwards velocity and set jump counter
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            MusicManager.Instance.RandomiseSfx(jumpSound);
            isJumping = true;
            animator.SetBool("Falling", true);
            animator.SetTrigger("Jumping");
            jumpTimeCounter = maxJumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        //if in the air and pressing the jump button, increase upwards velocity until counter is 0, then stop jumping
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        //if released the jump button, stop jumping
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    //Function for player climbing
    void Climb()
    {
        //get vertical input
        verticalMoveInput = Input.GetAxisRaw("Vertical");

        touchingLadder = Physics2D.OverlapBox(bodyPos.position, bodySize, 0, LayerMask.GetMask("Climbing"));

        //if colliding a climbing layer, start climbing
        if (touchingLadder)
        {

            //set vertical velocity to climb speed and keep x the same
            rb.velocity = new Vector2(rb.velocity.x, verticalMoveInput * climbSpeed);

            //set climbing animation based on if the player is moving upwards
            bool hasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
            animator.SetBool("Climbing", hasVerticalSpeed);

            //set gravity to 0 to stop falling while on the ladder
            rb.gravityScale = 0;
            animator.SetBool("Falling", false);

            //if player is touching the ground and they are moving down then set climb animation to false
            if (isGrounded && verticalMoveInput != 0)
            {
                animator.SetBool("Climbing", false);
            }

        }
        else
        {
            //if not colliding with climbing layer set gravity to normal and stop climbing animation
            rb.gravityScale = gravity;
            animator.SetBool("Climbing", false);
        }

    }

    //-----------------------------------------------------------------Bird Functions----------------------------------------------

    //Summons the bird and allows it to be resummoned after a delay. Set summoning to true. Also set gravity to 0 while summoning.
    void SummonBird()
    {
        if (summonTimer <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                summoning = true;
                rb.gravityScale = 0;
                animator.SetBool("Falling", false);
                animator.SetTrigger("SummonBird");
                MusicManager.Instance.RandomiseSfx(summonSound);
                CheckDash();
                summonTimer = summonDelay;
            }
        }
        else
        {
            summonTimer -= Time.deltaTime;
        }
    }

    //After the bird has been summoned, set the dash position based on direction facing. 
    //Check if the player can dash by raycasting to see if a wall is hit at the dash position. 
    //If so set canDash and summoning to false. Else set canDash to true and start dash.
    void CheckDash()
    {

        if (sr.flipX)
        {
            dashPos = leftDashPos.position;
            dashDirection = Vector2.left;
        }
        else
        {
            dashPos = rightDashPos.position;
            dashDirection = Vector2.right;
        }

        float dashDistance = Vector2.Distance(transform.position, dashPos);

        if (Physics2D.Raycast(transform.position, dashDirection, dashDistance, LayerMask.GetMask("Ground")))
        {
            canDash = false;
            summoning = false;
        }
        else
        {
            canDash = true;
        }

        animator.SetBool("BirdDash", canDash);
    }

    //Dash event called during BirdDash animation. Create bird explosion particles and set player position to dash pos. set summoning to false and set gravity back to normal.
    void Dash()
    {
        MusicManager.Instance.RandomiseSfx(deathSound);
        StartCoroutine(BirdExplosion());
        transform.position = dashPos;
        summoning = false;
        rb.gravityScale = gravity;
    }

    //Bird explositon coroutine
    IEnumerator BirdExplosion()
    {
        GameObject particles = Instantiate(birdExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(particles);
    }
}
