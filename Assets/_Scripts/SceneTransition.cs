using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "zone1_transition")
        {
            anim.SetTrigger("fadeOut");
            Invoke("ZoneOne", 1);
        }
    }

    private void ZoneOne()
    {
        SceneManager.LoadScene("scene_zone1");
    }
}
