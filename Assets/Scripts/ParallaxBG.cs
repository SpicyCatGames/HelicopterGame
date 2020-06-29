using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    [SerializeField] private Vector2 _parallaxMultiplier = new Vector2(0.5f, 0.5f);
    [SerializeField] private bool _infiniteVerticalScrolling = false;
    private Transform _transform;
    private Camera _cam;
    private Transform _camTransform;
    private Vector2 _camLastPos;
    private float textureUnitX;
    private float textureUnitY;


    void Start()
	{
        _cam = Camera.main;
        _transform = transform;
        _camTransform = _cam.transform;
        _camLastPos = _camTransform.position;

        textureUnitX = GetComponent<SpriteRenderer>().sprite.texture.width / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        textureUnitY = GetComponent<SpriteRenderer>().sprite.texture.height / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
    }

	void LateUpdate()
	{
        //parallax
        Vector2 camPosDelta = (Vector2)_camTransform.position - _camLastPos;
        _transform.position += new Vector3(camPosDelta.x * _parallaxMultiplier.x, camPosDelta.y * _parallaxMultiplier.y);
        _camLastPos = _camTransform.position;

        //handle infinite scrolling
        if (Mathf.Abs(_camTransform.position.x - _transform.position.x) > textureUnitX)
        {
            float offSet = (_camTransform.position.x - _transform.position.x) % textureUnitX;
            _transform.position = new Vector2(_camTransform.position.x + offSet, _transform.position.y);
        }
        if (_infiniteVerticalScrolling && Mathf.Abs(_camTransform.position.y - _transform.position.y) > textureUnitY)
        {
            float offSet = (_camTransform.position.y - _transform.position.y) % textureUnitY;
            _transform.position = new Vector2(_transform.position.x, _camTransform.position.y + offSet);
        }
	}
}