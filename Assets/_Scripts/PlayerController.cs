using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum MovementState
{
    idle,
    right,
    left,
    up,
    down
}

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    [Header("UI")]
    public StaminaBar stam;
    public HealthBar health;
    public Text moneyText;

    [Header("Attacking")]
    public float attackReach;
    public float attackRadius;
    public int startingAttackDamage;
    private int _attackDamage;

    [Header("Health")]
    public int startingMaxHealth;
    public int maxHealth;
    public int currentHealth;
    public int healthRegenAmount;

    [Header("Rifting")]
    public int riftStamCost;
    public int maxStam;
    public float riftDistance;
    public int startingRegenAmount;
    private int _stamRegenAmount;

    [Header("Upgrade Magnitudes")]
    public int healthUpgradeMagnitude;
    public int attackDamageUpgradeMagnitude;
    public int regenUpgradeMagnitude;

    [HideInInspector]
    public int healthUpgrade = 0;
    [HideInInspector]
    public int attackUpgrade = 0;
    [HideInInspector]
    public int regenUpgrade = 0;

    [Header("Upgrade Cost")]
    public int tier1cost;
    public int tier2cost;
    public int tier3cost;
    private int _money;

    [Header("Utility")]
    public MovementState state;
    public Animator playerAnim;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public GameObject riftTrails;
    public Sprite idleSprite;
    
    public bool waitingToMove = false;
    public bool mouseReadDisabled = false;
    bool waitingToRespawn = false;
    bool rifting = false;

    [HideInInspector]
    public Rigidbody2D rb;
    Vector2 movement;
    Vector2 archiveMovement;

    private void Awake()
    {
        RefreshUpgrades();

        health.SetHealth(maxHealth);
        health.SetHealthRegenAmount(healthRegenAmount);

        stam.SetStaminaRegenAmount(_stamRegenAmount);
        stam.SetMaxStamina(maxStam);

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        _money = 0;
    }

    public void RefreshUpgrades()
    {
        maxHealth = startingMaxHealth + (healthUpgrade * healthUpgradeMagnitude);
        currentHealth = maxHealth;

        _attackDamage = startingAttackDamage + (attackUpgrade * attackDamageUpgradeMagnitude);
        _stamRegenAmount = startingRegenAmount + (regenUpgrade * regenUpgradeMagnitude);

        health.SetMaxHealth(maxHealth);
        stam.SetStaminaRegenAmount(_stamRegenAmount);
    }

    public void UpgradeHealth()
    {
        if (healthUpgrade == 2)
        {
            _money -= tier3cost;
        }
        else if (healthUpgrade == 1)
        {
            _money -= tier2cost;
        }
        else if (healthUpgrade == 0)
        {
            _money -= tier1cost;
        }
        else return;

        AddCurrency(0);
        healthUpgrade++;
    }

    public void UpgradeAttack()
    {
        if (attackUpgrade == 2)
        {
            _money -= tier3cost;
        }
        else if (attackUpgrade == 1)
        {
            _money -= tier2cost;
        }
        else if (attackUpgrade == 0)
        {
            _money -= tier1cost;
        }
        else return;

        AddCurrency(0);
        attackUpgrade++;
    }

    public void UpgradeRegen()
    {
        if (regenUpgrade == 2)
        {
            _money -= tier3cost;
        }
        else if (regenUpgrade == 1)
        {
            _money -= tier2cost;
        }
        else if (regenUpgrade == 0)
        {
            _money -= tier1cost;
        }
        else return;

        AddCurrency(0);
        regenUpgrade++;
    }

    void Update()
    {
        if(!waitingToMove) movement.x = Input.GetAxisRaw("Horizontal");
        if(!waitingToMove) movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0 || movement.y != 0) archiveMovement = movement;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (waitingToRespawn) return;
            if (waitingToMove) return;            
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("trigger 1");
            if (waitingToRespawn) Respawn();
            Debug.Log("trigger 2");
            if (waitingToMove) return;
            Debug.Log("trigger 3");
            if (!stam.CheckIfEnoughStamina(riftStamCost)) return;
            Debug.Log("trigger 4");
            waitingToMove = true;
            RiftSetup();
        }

        if (Input.GetKeyDown(KeyCode.T)) TakeDamage(20);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        health.SetHealth(currentHealth);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        if (currentHealth <= 0)
        {
            Invoke("PlayerDeath", 0.15f);
        }
        else
        {
            Invoke("ReturnSpriteColor", 0.15f);
        }
    }

    public bool CanAfford(int tier)
    {
        if(tier == 3)
        {
            if (_money >= tier3cost) return true;
            else return false;
        }
        else if(tier == 2)
        {
            if (_money >= tier2cost) return true;
            else return false;
        }
        else if (tier == 1)
        {
            if (_money >= tier1cost) return true;
            else return false;
        }
        else
        {
            Debug.Log("Player: CanAfford: Unknown Tier: " + tier);
            return false;
        }
    }

    private void ReturnSpriteColor()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }        

    private void FixedUpdate()
    {
        Vector2 direction = this.gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(direction.x, direction.y) + 3.14159f/2 ;

        /*
        if ((angle <= 22.5 && angle > 0) || (angle > 337.5 && angle <= 0))  //top
        {
            attackPoint.localPosition = new Vector3(0, attackReach * 1, 0);
        }
        else if (angle <= 67.5 && angle > 22.5)                              //top - right
        {
            attackPoint.localPosition = new Vector3(attackReach * 0.7f, attackReach * 0.7f, 0);
        }
        else if (angle <= 112.5 && angle > 67.5)                             //right
        {
            attackPoint.localPosition = new Vector3(attackReach * 1, 0, 0);
        }
        else if (angle <= 157.5 && angle > 112.5)                            //bottom - right
        {
            attackPoint.localPosition = new Vector3(attackReach * 0.7f, attackReach * -1, 0);
        }
        else if (angle <= 202.5 && angle > 157.5)                            //bottom
        {
            attackPoint.localPosition = new Vector3(0, attackReach * -1.3f, 0);
        }
        else if (angle <= 247.5 && angle > 202.5)                            //bottom - left
        {            
            attackPoint.localPosition = new Vector3(attackReach * -0.7f, attackReach * -1, 0);
        }
        else if (angle <= 292.5 && angle > 247.5)                            //left
        {
            attackPoint.localPosition = new Vector3(attackReach * -1, 0, 0);
        }
        else if (angle <= 337.5 && angle > 292.5)                            //top - left
        {
            attackPoint.localPosition = new Vector3(attackReach * -0.7f, attackReach * 0.7f, 0);
        }        
        else                                                            //no direction
        {
            attackPoint.localPosition = new Vector3(0, attackReach * 1, 0);
        }
        */

        if (!mouseReadDisabled)
        {
            if (Vector2.Distance(this.gameObject.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) >= attackReach) attackPoint.localPosition = new Vector3(attackReach * Mathf.Cos(angle), -attackReach * Mathf.Sin(angle), 0);
            else attackPoint.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }

        if (rifting)
        {
            Rift();
        }
        else if (!waitingToMove)
        {
            if (movement.x != 0 && movement.y != 0) rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime * .7f);
            else rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

        playerAnim.SetFloat("moveX", movement.x);
        playerAnim.SetFloat("moveY", movement.y);

        if (movement.x == 0) playerAnim.SetBool("horizontalMove", false);
        else playerAnim.SetBool("horizontalMove", true);

        if (movement.y == 0) playerAnim.SetBool("verticalMove", false);
        else playerAnim.SetBool("verticalMove", true);
    }
    
    private void Attack()
    {
        //anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<Enemy>().takeDamage(_attackDamage);
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
        Invoke("RespawnTrigger", 1.5f);
    }

    public void AddCurrency(int newMoney)
    {
        _money += newMoney;

        if(_money < 10) moneyText.text = "000" + _money;
        else if(_money < 100) moneyText.text = "00" + _money;
        else if (_money < 1000) moneyText.text = "0" + _money;
        else moneyText.text = "" + _money;
    }

    private void RespawnTrigger()
    {
        Debug.Log("Respawn Trigger");
        waitingToRespawn = true;
    }

    private void Respawn()
    {
        this.gameObject.GetComponent<SceneTransition>().RespawnPlayer();
    }

    public void SetWaitingToMove(bool log)
    {
        waitingToMove = log;
    }

    public void SetMouseReadLock(bool log)
    {
        mouseReadDisabled = log;
    }
}
