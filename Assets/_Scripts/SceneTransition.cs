using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    
    public GameObject respawnText;
    public GameObject spacebarText;
    public CanvasGroup sceneGroup;

    private bool _transitioning;

    [Header("Keep throughout scenes:")]
    public GameObject cineCam;
    public Animator anim;
    public Canvas transitionCanvas;
    public Canvas uiCanvas;

    private void Awake()
    {        
        SceneTransition _test = GameObject.FindGameObjectWithTag("Player").GetComponent<SceneTransition>();
        if (_test != this)
        {
            Destroy(anim);
            Destroy(_test.gameObject);
            Destroy(uiCanvas);
            Destroy(transitionCanvas);
            Destroy(cineCam);
        }
        else
        {
            DontDestroyOnLoad(anim);
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(uiCanvas);
            DontDestroyOnLoad(transitionCanvas);
            DontDestroyOnLoad(cineCam);
        }
        _transitioning = false;
        Debug.Log("SceneTrans: Awake()");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_transitioning) return;
        if(collision.gameObject.tag == "Transition")
        {
            anim.SetTrigger("fadeIn");
            _transitioning = true;

            if (collision.gameObject.name == "zone1_transition")
            {                
                Invoke("ZoneOne", 1.4f);                
            }

            Invoke("Spawn", 1.5f);
            Invoke("RevealScene", 1.6f);
        }
        
    }

    private void ZoneOne()
    {
        SceneManager.LoadScene("prototyping_scene0");   
        Debug.Log("SceneTrans: SceneLoad()");
    }

    private void Spawn()
    {        
        Transform spawnPos = null;
        if (GameObject.FindGameObjectWithTag("SpawnPoint").transform != null) spawnPos = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        this.transform.position = spawnPos.position;        
        Debug.Log("SceneTrans: Spawn/Fade Out Black()");
    }

    private void RevealScene()
    {
        anim.SetTrigger("fadeOut");
        _transitioning = false;
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
