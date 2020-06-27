using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUIPlacement : MonoBehaviour
{
    [SerializeField] private float _zPosition = 1;

    [SerializeField] private TouchInput _touchInput = null;
    Camera cam;
    float scale;
    float camStartSize;
    private Vector3 startScale;

	void Start()
	{
        cam = Camera.main;
        camStartSize = cam.orthographicSize;
        CalculateScale();
        startScale = transform.localScale;
	}

	void LateUpdate()
	{
        //calculate this position here
        Vector2 controllerWorldPos = cam.ViewportToWorldPoint(_touchInput._originViewport);
        transform.position = new Vector3(controllerWorldPos.x, controllerWorldPos.y, _zPosition);
        transform.localScale = startScale * (cam.orthographicSize / camStartSize);
	}

    private void CalculateScale()
    {
        float height = GetComponent<SpriteRenderer>().sprite.texture.height;
        float ppu = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        float sizeFromHeightWorld = height / ppu;//this is assuming scale is 1, just in case I decide to use this formula later, here it works at any starting scale
        float displayPixels = sizeFromHeightWorld * SilverUtils.Misc.GetPPU(cam);
        float targetHeight = _touchInput._sizeFromHeight * Screen.height;
        scale = targetHeight / displayPixels;
        transform.localScale = new Vector3(scale, scale, transform.localScale.z);
    }
}