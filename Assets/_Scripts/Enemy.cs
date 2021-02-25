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
    private Color _tint;

    [Header("Balancing")]
    public int deathValue;
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
        _tint = _rend.color;

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
            Invoke("EnemyDeath", 0.15f);
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
            Debug.Log("ATTACKED PLAYER");
            _player.TakeDamage(attackDamage);
        }
    }

    public bool isCooledDown()
    {
        Debug.Log("Cooldown: " + _cooldownStatus);
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
        Vector2 direction = (_player.transform.position - this.transform.position);
        float angle = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg) + 180;
        
        enemyAnim.SetInteger("angle", (int)angle);
    }

    private void EnemyDeath()
    {
        Invoke("SelfDestruct", 0.05f);
        _player.AddCurrency(deathValue);
    }

    private void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    private void ReturnSpriteColor()
    {
        _rend.color = _tint;
    }
}
