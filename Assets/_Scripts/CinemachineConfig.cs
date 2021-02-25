using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineConfig : MonoBehaviour
{
    private GameObject _CMvcam1;

    private CinemachineVirtualCamera _camera;
    private CinemachineConfiner _bounds;

    public Transform player;

    private void Awake()
    {
        _CMvcam1 = this.gameObject;

        _camera = _CMvcam1.GetComponent<CinemachineVirtualCamera>();
        _bounds = _CMvcam1.GetComponent<CinemachineConfiner>();
    }

    void Start()
    {
        _camera.Follow = player;
        _bounds.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Bounds_main").GetComponent<PolygonCollider2D>();
    }    

    public void LoadScene(string scene)
    {
        _camera.Follow = player;

        if(scene == "mainRoom") _bounds.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Bounds_main").GetComponent<PolygonCollider2D>();
        if (scene == "zone1") _bounds.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Bounds_zone1").GetComponent<PolygonCollider2D>();
    }
}
