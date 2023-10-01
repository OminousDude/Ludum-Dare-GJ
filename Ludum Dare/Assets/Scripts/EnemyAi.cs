using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    [SerializeField] float moveSpeed = 2f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    public bool grounded = false;

    float t = 0f;
    [SerializeField][Range(0f, 4f)] float lerpTime = 4f;
    bool isCorrecting = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("EnemyTarget").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation.Set(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);

        if (isCorrecting) {
            rb.transform.eulerAngles = Vector2.Lerp(rb.transform.eulerAngles, new Vector2(0, 0), lerpTime * Time.deltaTime);

            t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

            if (t > 0.9f)
            {
                t = 0f;
                isCorrecting = false;
                rb.freezeRotation = true;
            }
        }

        if (target && grounded && !isCorrecting)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            // Debug.Log("Direction x : " + direction.x + "; Angle : " + angle);
            // rb.rotation = angle;
            rb.transform.eulerAngles = new Vector2(0, direction.x > 0 ? 180 : 0);
            moveDirection = direction;
        }
    }

    private void FixedUpdate()
    {
        if (target && grounded)
        {
            rb.velocity = new Vector2(moveDirection.x, 0) * moveSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 && !grounded)
        {
            grounded = true;
            isCorrecting = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            grounded = false;
            rb.freezeRotation = false;
            isCorrecting = false;
        }
    }
}
