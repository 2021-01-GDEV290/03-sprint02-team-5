using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController player;
    private int _counter = 0;
    private int _healthRegenAmount;
    public Slider slider;

    public Text healthPoints;

    private void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
    }

    public void SetHealthRegenAmount(int newRegenAmount)
    {
        _healthRegenAmount = newRegenAmount;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        slider.maxValue = newMaxHealth;
        slider.value = slider.maxValue;
        UpdateHealthPercentage();
    }
    
    public void SetHealth(int newHealth)
    {
        slider.value = newHealth;
        UpdateHealthPercentage();
    }   
    
    private void UpdateHealthPercentage()
    {
        healthPoints.text = (int)slider.value + "hp";
    }

    private void FixedUpdate()
    {
        if (slider.maxValue > slider.value) _counter++;

        if (_counter >= 50)
        {
            slider.value += _healthRegenAmount;
            UpdateHealthPercentage();
            _counter = 0;
        }
    }
}
