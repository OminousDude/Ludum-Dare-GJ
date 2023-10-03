using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isAlive;

    private Collision coll;
    [HideInInspector]
    public Rigidbody2D rb;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    [Space]

    private bool isGrounded;
    private int isJumping;
    private bool canJump;
    private bool groundTouch;
    private bool hasDashed;
    private bool playerHit = false;

    public int side = 1;

    public Animator anim;
    public float hitWaitTime;

    private void Awake()
    {
        //GameManager.onStateChange += OnGameStateChange;
        isAlive = true; 
    }

    private void OnGameStateChange(GameManager.GameState obj)
    {
        isAlive = obj == GameManager.GameState.Alive || obj == GameManager.GameState.Warning || obj == GameManager.GameState.LevelTransition;
    }
    public void OnDestroy()
    {
        GameManager.onStateChange -= OnGameStateChange;
    }
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                float xRaw = Input.GetAxisRaw("Horizontal");
                float yRaw = Input.GetAxisRaw("Vertical");
                Vector2 dir = new Vector2(x, y);

                anim.SetBool("Walk", dir.x + dir.y != 0 && hitWaitTime <= 0);

                if (hitWaitTime <= 0)
                {
                    anim.SetBool("Hit", false);
                }

                if (hitWaitTime > 0)
                {
                    anim.SetBool("Hit", true);
                    hitWaitTime-=Time.deltaTime;
                }

            Walk(dir);

                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (isGrounded || canJump)
                    {
                        Jump(Vector2.up, false);
                        canJump = false;
                    }
                }

                if (coll.onGround && !groundTouch)
                {
                    GroundTouch();
                    groundTouch = true;
                }

                if (!coll.onGround && groundTouch)
                {
                    groundTouch = false;
                }

                if (x > 0)
                {
                    side = 1;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                if (x < 0)
                {
                    side = -1;

                    GetComponent<SpriteRenderer>().flipX = false;
                }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            isGrounded = true;
            isJumping = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = false;
            if (isJumping == 0)
            {
                canJump = true;
                isJumping++;
            }
        }
    }
    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;
    }

    private void Walk(Vector2 dir)
    {
        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector2 dir, bool wall)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }

    
}