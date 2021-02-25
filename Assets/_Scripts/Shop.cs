using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public LayerMask scanLayer;
    public float scanRadius;

    public GameObject interactPrompt;
    public GameObject upgradeMenu;
    public Canvas sceneCrossfade;

    void Update()
    {
        Collider2D playerScan = Physics2D.OverlapCircle(this.gameObject.transform.position - new Vector3(0, 0.5f, 0), scanRadius, scanLayer);

        if(playerScan != null)
        {
            interactPrompt.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerScan.gameObject.GetComponent<PlayerController>().waitingToMove = true;
                playerScan.gameObject.GetComponent<PlayerController>().mouseReadDisabled = true;

                sceneCrossfade.enabled = false;
                interactPrompt.SetActive(false);
                upgradeMenu.SetActive(true);
                upgradeMenu.GetComponent<TierManager>().RefreshTiers();
            }
        }
        else
        {
            interactPrompt.SetActive(false);
            upgradeMenu.SetActive(false);
        }
    }
}
