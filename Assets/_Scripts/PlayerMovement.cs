﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float riftDistance;

    public GameObject riftTrails;
    public Sprite idleSprite;
    
    bool rifting = false;
    bool waitToMove = false;



    [HideInInspector]
    public Rigidbody2D rb;
    Vector2 movement;
    Vector2 archiveMovement;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            archiveMovement = movement;
            waitToMove = true;
            RiftSetup();
        }
    }

    private void FixedUpdate()
    {        
        if (rifting)
        {
            Rift();
        }
        else if (!waitToMove)
        {
            if (movement.x != 0 && movement.y != 0) rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime * .7f);
            else rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void RiftSetup()
    {
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
        waitToMove = false;
        Invoke("RiftTrailCleanup", 0.25f);
    }

    private void RiftTrailCleanup()
    {
        riftTrails.SetActive(false);
    }
}