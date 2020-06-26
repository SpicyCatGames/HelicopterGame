using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _player;
    [SerializeField] private Vector2 _offsetViewport = default;
    [SerializeField][Range(0, 1)]private float _smoothTime = 0.005f;
    [SerializeField]private float _clamptoMinY = -10f;
    Camera cam;
    Transform _transform;
    private Vector3 _velocity = Vector3.zero;
    private void Start()
    {
        cam = Camera.main;
        _transform = transform;

    }
    private void LateUpdate()
    {
        Vector2 playerViewport = cam.WorldToViewportPoint(_player.position);
        Vector2 cameraPosition = cam.ViewportToWorldPoint(playerViewport - _offsetViewport);

        //limit the min y so that the bottom of the camera is clamped to a y value
        //we will get the imaginery center of the camera if the bottom was at float _clamptoMinY, then clamp cam y low to that
        float camY = Mathf.Clamp(cameraPosition.y, _clamptoMinY + cam.orthographicSize, Mathf.Infinity);

        Vector3 camPosV3 = new Vector3(cameraPosition.x, camY, _transform.position.z);
        _transform.position = Vector3.SmoothDamp(_transform.position, camPosV3, ref _velocity, _smoothTime);
    }
}
