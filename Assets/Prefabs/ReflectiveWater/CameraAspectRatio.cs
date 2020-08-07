using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraAspectRatio : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private RenderTexture _tex = default;
    private void Start()
    {
        UpdateAspectRatio();
    }

    [ContextMenu("UpdateAspectRatio()")]
    private void UpdateAspectRatio()
    {
        _cam = GetComponent<Camera>();
        _cam.aspect = _tex.width / _tex.height;
    }
}
