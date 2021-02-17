using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    [Header("UI")]
    public StaminaBar stam;
    public HealthBar health;

    [Header("Attacking")]
    public int maxHealth;
    public int healthRegenAmount;

    [Header("Rifting")]
    public int riftStamCost;
    public int maxStam;
    public int stamRegenAmount;
    public float riftDistance;

    [Header("Utility")]
    public GameObject riftTrails;
    public Sprite idleSprite;
    
    bool rifting = false;
    bool waitingToMove = false;
    bool waitingForRespawn = false;



    [HideInInspector]
    public Rigidbody2D rb;
    Vector2 movement;
    Vector2 archiveMovement;

    private void Awake()
    {
        health.SetMaxHealth(maxHealth);
        health.SetHealthRegenAmount(healthRegenAmount);

        stam.SetStaminaRegenAmount(stamRegenAmount);
        stam.SetMaxStamina(maxStam);

        rb = this.gameObject.GetComponent<Rigidbody2D>();        
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (waitingForRespawn) Respawn();
            if (waitingToMove) return;
            if (!stam.CheckIfEnoughStamina(riftStamCost)) return;
            archiveMovement = movement;
            waitingToMove = true;
            RiftSetup();
        }

        if (Input.GetKeyDown(KeyCode.T)) InflictTestDamage();
    }

    private void InflictTestDamage()
    {
        health.takeDamage(10);
    }

    private void FixedUpdate()
    {
        if (rifting)
        {
            Rift();
        }
        else if (!waitingToMove)
        {
            if (movement.x != 0 && movement.y != 0) rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime * .7f);
            else rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void RiftSetup()
    {
        stam.UseStamina(riftStamCost);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        riftTrails.SetActive(true);
        Invoke("RiftTrigger", 0.1f);
    }

    private void RiftTrigger()
    {
        rifting = true;
    }

    private void Rift()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;

        if (archiveMovement.x == 0 && archiveMovement.y == 0) archiveMovement.y = 1; //default teleport

        if(archiveMovement.x != 0 && archiveMovement.y != 0) rb.MovePosition(rb.position + archiveMovement * riftDistance * 0.7f);
        else rb.MovePosition(rb.position + archiveMovement * riftDistance);
        
        rifting = false;
        waitingToMove = false;
        Invoke("RiftTrailCleanup", 0.25f);
    }

    private void RiftTrailCleanup()
    {
        riftTrails.SetActive(false);
    }

    public void PlayerDeath()
    {
        this.gameObject.GetComponent<SceneTransition>().PlayerDeath();        
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        waitingToMove = true;
        Invoke("RespawnTrigger", 1.7f);
    }

    private void RespawnTrigger()
    {
        waitingForRespawn = true;
    }

    private void Respawn()
    {
        this.gameObject.GetComponent<SceneTransition>().RespawnPlayer();
    }
}
