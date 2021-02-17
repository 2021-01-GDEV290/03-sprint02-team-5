using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private int counter = 0;
    private int stamRegenAmount;
    private Slider slider;

    public Text riftPercentage;

    private void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
    }
    
    public void SetStaminaRegenAmount(int newRegenAmount)
    {
        stamRegenAmount = newRegenAmount;
    }

    public void SetMaxStamina(int newMaxStamina)
    {
        slider.maxValue = newMaxStamina;
        slider.value = slider.maxValue;
        UpdateStamPercentage();
    }
    
    public void UseStamina(int usedAmount)
    {
        slider.value -= usedAmount;
        UpdateStamPercentage();
    }

    public bool CheckIfEnoughStamina(int moveCost)
    {
        if (slider.value >= moveCost) return true;
        else return false;
    }

    private void UpdateStamPercentage()
    {
        riftPercentage.text = (int)((slider.value / slider.maxValue) * 100) + "%";
    }

    private void FixedUpdate()
    {
        if (slider.maxValue > slider.value) counter++;

        if(counter >= 20)
        {
            slider.value += stamRegenAmount;
            UpdateStamPercentage();
            counter = 0;
        }
    }
}
