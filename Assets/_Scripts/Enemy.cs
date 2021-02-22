using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public EnemyHealthBar healthBar;
    public int maxHealth;
    private int _currentHealth;
    private SpriteRenderer _rend;

    private void Awake()
    {
        healthBar = this.transform.GetChild(0).GetChild(0).GetComponent<EnemyHealthBar>();
        _currentHealth = maxHealth;
        _rend = this.gameObject.GetComponent<SpriteRenderer>();

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

    private void NullSprite()
    {
        _rend.sprite = null;
    }

    private void ReturnSpriteColor()
    {
        _rend.color = Color.white;
    }
}
