using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController player;
    public Slider slider;

    public Text healthPoints;

    private void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
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
}
