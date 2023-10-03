using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceTest : MonoBehaviour
{
    public float bounce = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();

        Vector2 direction = Vector2.Reflect(enemyRB.velocity.normalized, collision.contacts[0].normal);

        enemyRB.AddForce(new Vector2(0.7f, 0.5f) * bounce, ForceMode2D.Impulse);
    }
}
