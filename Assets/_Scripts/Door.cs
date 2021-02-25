using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string keyColor;

    [Header("Util")]
    public LayerMask scanLayer;
    public float scanRadius;

    [Header("UI")]
    public GameObject getKeyPrompt;
    public GameObject interactPrompt;

    private PlayerController _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        Collider2D playerScan = Physics2D.OverlapCircle(this.gameObject.transform.position, scanRadius, scanLayer);

        if (playerScan != null)
        {
            if (CheckKey(keyColor))
            {
                interactPrompt.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactPrompt.SetActive(false);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                getKeyPrompt.SetActive(true);
            }            
        }
        else
        {
            getKeyPrompt.SetActive(false);
            interactPrompt.SetActive(false);
        }
    }

    private bool CheckKey(string keyColor)
    {
        bool check = false;

        if (keyColor == "Green")
        {
             check = _player.hasGreenKey();
        }
        if (keyColor == "Blue")
        {
            check = _player.hasBlueKey();
        }
        if (keyColor == "Purple")
        {
            check = _player.hasPurpleKey();
        }
        return check;
    }
}
