using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    Rigidbody2D rb;
    public GameObject hb;
    public bool isHit;
    public float time;
    public bool canBeHit;
    bool isLeftHit = true;
    [SerializeField] AudioSource hitAudio;
    [SerializeField] FloatingHealthBar healthBar;

    [SerializeField] float bounce = 50f;
    public EnemyAi enemyAi;
    public Animator anim;

    private GameManager gameManager;
    public ParticleSystem particle;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    // Start is called before the first frame update
    void Start()
    {
        canBeHit = false;
        isHit = false;
        hb.SetActive(false);
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && GameObject.Find("Player").GetComponentInChildren<PlayerMovement>().hitWaitTime == 0)
        {
            GameObject.Find("Player").GetComponentInChildren<PlayerMovement>().hitWaitTime = 350;
            if (canBeHit)
            {
                time = 0;
                time += Time.deltaTime;
                //Debug.Log(time);
                hb.SetActive(true);
                TakeDamage(1f);
                isHit = true;
                Vector2 directionVec = isLeftHit ? new Vector2(0.7f, 0.5f) : new Vector2(-0.7f, 0.5f);
                enemyAi.grounded = false;
                rb.AddForce(directionVec * bounce, ForceMode2D.Impulse);
            }
        }
        else if (isHit)
        {
            time += Time.deltaTime;
            //Debug.Log(time);
        }
        if (time > 1)
        {
            hideHealth();
            time = 0;
            isHit = false;
        }

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            canBeHit = true;
            isLeftHit = (collision.gameObject.transform.position - transform.position).normalized.x < 0;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
         if (collision.gameObject.layer == 8)
        {
            canBeHit = false;
        }
    }
    public void hideHealth()
    {
        hb.SetActive(false);
    }
    public void TakeDamage(float damageAmount)
    {
        hitAudio.Play();
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        //GetComponent<LootBag>().InstantiateLoot(transform.position);
        gameManager.numberEnemies--;
        gameManager.numberDeadEnemies--;
        transform.position.Set(transform.position.x, transform.position.y + 1, transform.position.z);
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(rb);
    }
}