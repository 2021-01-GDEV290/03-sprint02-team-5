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
        _bounds.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Bounds").GetComponent<PolygonCollider2D>();
    }    

    public void LoadScene()
    {
        _camera.Follow = player;
        _bounds.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Bounds").GetComponent<PolygonCollider2D>();
    }
}
