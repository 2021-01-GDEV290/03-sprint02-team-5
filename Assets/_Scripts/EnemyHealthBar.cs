using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Vector3 healthBarOffset;
    private Enemy _enemy;   //for position tracking
    private Slider _slider;

    private void Awake()
    {        
        _slider = this.gameObject.GetComponent<Slider>();
    }

    public void SetParentEnemy(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        _slider.maxValue = newMaxHealth;
    }

    public void SetHealth(int newHealth)
    {
        _slider.value = newHealth;
    }

    public void OnDeath()
    {
        this.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (_enemy == null)
        {
            Destroy(this.gameObject);
            return;
        }
        _slider.transform.position = Camera.main.WorldToScreenPoint(_enemy.transform.position + healthBarOffset);
    }
}
