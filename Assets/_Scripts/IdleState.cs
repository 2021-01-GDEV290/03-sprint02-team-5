using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chase;
    public bool detectingPlayer;

    public override State RunCurrentState()
    {
        Enemy enemy = this.transform.parent.GetComponent<Enemy>();
        Collider2D player = Physics2D.OverlapCircle(enemy.gameObject.transform.position, enemy.detectionRadius, enemy.attackLayer);

        if (player != null) detectingPlayer = true;
        else detectingPlayer = false;

        Debug.Log("Enemy State: Idle");


        if (detectingPlayer)
        {
            return chase;
        }
        else
        {
            return this;
        }
    }
}
