using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chase;
    public bool playerInRange;

    public override State RunCurrentState()
    {
        Enemy enemy = this.transform.parent.GetComponent<Enemy>();
        Collider2D attackScan = Physics2D.OverlapCircle(enemy.gameObject.transform.position - new Vector3(0,0.5f,0), enemy.attackRadius, enemy.attackLayer);

        if (attackScan != null) playerInRange = true;
        else playerInRange = false;

        Vector2 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - enemy.gameObject.transform.position);
        
        if(!playerInRange)
        {
            return chase;
        }
        else
        {
            enemy.AttackPlayer();
            return this;
        }

    }
}
