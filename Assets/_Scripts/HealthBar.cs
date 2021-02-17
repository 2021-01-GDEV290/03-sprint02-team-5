﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController player;
    private int counter = 0;
    private int healthRegenAmount;
    private Slider slider;

    public Text healthPercentage;

    private void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
    }

    public void SetHealthRegenAmount(int newRegenAmount)
    {
        healthRegenAmount = newRegenAmount;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        slider.maxValue = newMaxHealth;
        slider.value = slider.maxValue;
        UpdateHealthPercentage();
    }

    

    public void takeDamage(int damage)
    {
        slider.value -= damage;
        UpdateHealthPercentage();

        if(slider.value <= 0)
        {
            player.PlayerDeath();
        }
    }

    

    private void UpdateHealthPercentage()
    {
        healthPercentage.text = (int)((slider.value / slider.maxValue) * 100) + "%";
    }

    private void FixedUpdate()
    {
        if (slider.maxValue > slider.value) counter++;

        if (counter >= 50)
        {
            slider.value += healthRegenAmount;
            UpdateHealthPercentage();
            counter = 0;
        }
    }
}