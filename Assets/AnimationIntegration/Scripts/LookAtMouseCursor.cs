using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouseCursor : MonoBehaviour
{
    Camera _camera;
    private Vector3 pos;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        pos = _camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
