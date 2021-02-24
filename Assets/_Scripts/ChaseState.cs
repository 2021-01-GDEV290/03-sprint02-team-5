using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attack;
    public IdleState idle;
    public bool playerInRange;
    public bool detectingPlayer;

    public override State RunCurrentState()
    {
        Enemy enemy = this.transform.parent.GetComponent<Enemy>();
        Collider2D detectScan = Physics2D.OverlapCircle(enemy.gameObject.transform.position - new Vector3(0, 0.5f, 0), enemy.detectionRadius, enemy.attackLayer);

        if (detectScan != null) detectingPlayer = true;
        else detectingPlayer = false;

        Collider2D attackScan = Physics2D.OverlapCircle(enemy.gameObject.transform.position - new Vector3(0, 0.5f, 0), enemy.attackRadius - 0.1f, enemy.attackLayer);

        if (attackScan != null) playerInRange = true;
        else playerInRange = false;

        Vector2 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - enemy.gameObject.transform.position);
        enemy.gameObject.transform.Translate(direction * enemy.speed * Time.deltaTime);
        
        if (playerInRange)
        {
            enemy.enemyAnim.SetBool("isMoving", false);
            return attack;
        }
        else if (!detectingPlayer)
        {
            enemy.enemyAnim.SetBool("isMoving", false);
            return idle;
        }
        else
        {
            enemy.enemyAnim.SetBool("isMoving", true);
            return this;
        }
    }
}
