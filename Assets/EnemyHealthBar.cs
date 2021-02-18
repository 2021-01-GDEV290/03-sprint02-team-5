using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Enemy enemy;
    public Vector3 healthBarOffset;
    private Slider slider;

    private void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        slider.maxValue = newMaxHealth;
        slider.value = slider.maxValue;
    }

    public void SetHealth(int newHealth)
    {
        slider.value = newHealth;

        if(newHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        this.gameObject.transform.position = enemy.gameObject.transform.position + healthBarOffset;
    }
}
