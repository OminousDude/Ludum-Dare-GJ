using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D rb;
    public GameObject hb;
    public bool isHit;
    public float time;

    float speed = 0.01f;
    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        hb.SetActive(false);
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            time = 0;
            time += Time.deltaTime;
            //Debug.Log(time);
            hb.SetActive(true);
            TakeDamage(0.1f);
            isHit = true;

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
    public void hideHealth()
    {
        hb.SetActive(false);
    }
    public void TakeDamage(float damageAmount)
    {
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
        Destroy(gameObject);
        Destroy(rb);
    }
}