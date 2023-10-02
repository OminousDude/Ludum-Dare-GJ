using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool instIsHidden;
    private bool pauseIsHidden;

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
    public bool moving;

    [Space]

    private bool groundTouch;
    private bool hasDashed;

    public float hitRunTimer;

    public int side = 1;

    public Animator anim;

    private void Awake()
    {
        GameManager.onStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameManager.GameState obj)
    {
        instIsHidden = obj != GameManager.GameState.InstructionsMenu;
        pauseIsHidden = obj != GameManager.GameState.PauseMenu;
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
        if (instIsHidden || pauseIsHidden)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float xRaw = Input.GetAxisRaw("Horizontal");
            float yRaw = Input.GetAxisRaw("Vertical");
            Vector2 dir = new Vector2(x, y);

            anim.SetBool("Walk", (dir.x + dir.y != 0));

            if (hitRunTimer == 600) {
                anim.SetBool("Hit", true);
            }
            if (hitRunTimer == 0) {
                anim.SetBool("Hit", false);
            }
            if (hitRunTimer != 0) {
                hitRunTimer--;
            }


            moving = dir.x + dir.y != 0;

            Walk(dir);

            if (Input.GetButtonDown("Jump"))
            {
                if (coll.onGround)
                    Jump(Vector2.up, false);
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