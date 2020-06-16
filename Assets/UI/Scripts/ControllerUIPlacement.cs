using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUIPlacement : MonoBehaviour
{
    [SerializeField] private float _zPosition = 1;

    [SerializeField] private TouchInput _touchInput = null;
    Camera cam;
    float scale;

	void Start()
	{
        cam = Camera.main;
	}

	void Update()
	{
        //calculate this position here
        Vector2 controllerWorldPos = cam.ViewportToWorldPoint(_touchInput._originViewport);
        transform.position = new Vector3(controllerWorldPos.x, controllerWorldPos.y, _zPosition);
	}
}