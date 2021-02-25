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
        GameObject[] _test = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject test in _test)
        {
            if (test.GetComponent<SceneTransition>() != this)
            {
                SceneTransition purge = test.GetComponent<SceneTransition>();
                Destroy(purge.anim);                
                Destroy(purge.uiCanvas);
                Destroy(purge.transitionCanvas);
                Destroy(purge.cineCam);
                Destroy(test);
            }
        }
        DontDestroyOnLoad(anim);
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(uiCanvas);
        DontDestroyOnLoad(transitionCanvas);
        DontDestroyOnLoad(cineCam);

        _transitioning = false;
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
            if(collision.gameObject.name == "mainRoom_transition")
            {
                Invoke("MainRoom", 1.4f);
            }

            Invoke("Spawn", 1.5f);
            Invoke("RevealScene", 1.6f);
        }

        if(collision.gameObject.tag == "GreenKey")
        {
            this.gameObject.GetComponent<PlayerController>().GrabGreenKey();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "BlueKey")
        {
            this.gameObject.GetComponent<PlayerController>().GrabBlueKey();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "PurpleKey")
        {
            this.gameObject.GetComponent<PlayerController>().GrabPurpleKey();
            Destroy(collision.gameObject);
        }        
    }

    private void ZoneOne()
    {
        SceneManager.LoadSceneAsync("scene_zone1", LoadSceneMode.Additive);
        Invoke("ConfigZone1Cam", .5f);
    }

    private void MainRoom()
    {
        SceneManager.LoadSceneAsync("scene_mainRoom",LoadSceneMode.Additive);
        Invoke("ConfigMainRoomCam", .5f);
    }

    private void ConfigZone1Cam()
    {
        cineCam.GetComponent<CinemachineConfig>().LoadScene("zone1");
    }

    private void ConfigMainRoomCam()
    {
        cineCam.GetComponent<CinemachineConfig>().LoadScene("mainRoom");
    }

    private void Spawn()
    {        
        Transform spawnPos = null;
        if (GameObject.FindGameObjectWithTag("SpawnPoint").transform != null) spawnPos = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        this.transform.position = spawnPos.position;
        
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
        Invoke("SpacebarText", 1.5f);
    }

    private void SpacebarText()
    {
        spacebarText.SetActive(true);
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene(0);
    }
}
