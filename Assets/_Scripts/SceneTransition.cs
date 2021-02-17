using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator anim;
    public GameObject respawnText;
    public GameObject spacebarText;
    public CanvasGroup sceneGroup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "zone1_transition")
        {
            anim.SetTrigger("fadeIn");
            Invoke("ZoneOne", 1);
        }
    }

    private void ZoneOne()
    {
        SceneManager.LoadScene("scene_zone1");
    }

    public void PlayerDeath()
    {
        respawnText.SetActive(true);
        anim.SetTrigger("fadeIn");
        sceneGroup.alpha = 1;
        Invoke("SpacebarText", 1.7f);
    }

    private void SpacebarText()
    {
        spacebarText.SetActive(true);
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
