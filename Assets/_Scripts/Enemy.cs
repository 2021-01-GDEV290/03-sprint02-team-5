using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyHealthBar healthBar;
    public int maxHealth;
    private int currentHealth;
    private SpriteRenderer rend;

    private void Awake()
    {
        currentHealth = maxHealth;
        rend = this.gameObject.GetComponent<SpriteRenderer>();

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        rend.color = Color.red;

        if (currentHealth <= 0)
        {            
            Invoke("NullSprite", 0.15f);
        }
        else
        {
            Invoke("ReturnSpriteColor", 0.15f);
        }
    }

    private void NullSprite()
    {
        rend.sprite = null;
    }

    private void ReturnSpriteColor()
    {
        rend.color = Color.white;
    }
}
