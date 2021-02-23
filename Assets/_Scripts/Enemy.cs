using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyHealthBarPrefab;
    public Animator enemyAnim;

    [HideInInspector]
    public EnemyHealthBar healthBar;
    public int maxHealth;
    private int _currentHealth;
    private Rigidbody2D _rb;
    private SpriteRenderer _rend;
    private PlayerController _player;

    [Header("Balancing")]
    public int attackDamage;
    public float attackCooldown;
    private float _cooldownStatus;
    public float speed;

    [Header("State Machine")]
    public LayerMask attackLayer;
    public float detectionRadius;
    public float attackRadius;

    private void Awake()
    {
        GameObject instance = Instantiate(enemyHealthBarPrefab, GameObject.FindGameObjectWithTag("UI").transform);
        healthBar = instance.GetComponent<EnemyHealthBar>();
        healthBar.SetParentEnemy(this);
        _currentHealth = maxHealth;
        _rend = this.gameObject.GetComponent<SpriteRenderer>();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(_currentHealth);
    }

    public void takeDamage(int damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);
        _rend.color = Color.red;

        if (_currentHealth <= 0)
        {
            healthBar.OnDeath();
            Invoke("NullSprite", 0.15f);
        }
        else
        {
            Invoke("ReturnSpriteColor", 0.15f);
        }
    }

    public void AttackPlayer()
    {
        if(isCooledDown())
        {
            _player.TakeDamage(attackDamage);
            Debug.Log("Instakill probably");
        }
    }

    public bool isCooledDown()
    {
        _cooldownStatus -= Time.deltaTime;
        if (_cooldownStatus <= 0)
        {
            _cooldownStatus = attackCooldown;
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        enemyAnim.SetFloat("moveX", _rb.velocity.x);
        enemyAnim.SetFloat("moveY", _rb.velocity.y);
        if (_rb.velocity.y == 0)
        {
            enemyAnim.SetBool("verticalMove", false);
            if (_rb.velocity.x == 0) enemyAnim.SetBool("isMoving", false);
            else enemyAnim.SetBool("isMoving", true);
        }
        else enemyAnim.SetBool("verticalMove", true);
    }

    private void NullSprite()
    {
        //add currency to player
        Destroy(this.gameObject);
    }

    private void ReturnSpriteColor()
    {
        _rend.color = Color.white;
    }
}
