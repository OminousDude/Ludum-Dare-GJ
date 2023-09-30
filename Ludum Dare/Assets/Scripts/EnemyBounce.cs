using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : MonoBehaviour
{
    public bool shouldBounce = true;
    private int bounceCount = 0;

    [SerializeField]
    private Rigidbody2D rigidBody;

    private Vector2 velocity;

    private void Update()
    {
        velocity = rigidBody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (shouldBounce && collision.gameObject.layer == 6)
        {
            float speed = velocity.magnitude;

            Vector2 direction = Vector2.Reflect(velocity.normalized, collision.contacts[0].normal);

            rigidBody.velocity = direction * Mathf.Max(speed, 0);
        } else if (shouldBounce && collision.gameObject.layer == 3)
        {
            shouldBounce = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!shouldBounce && collision.gameObject.layer == 3)
        {
            shouldBounce = true;
        }
    }
}
