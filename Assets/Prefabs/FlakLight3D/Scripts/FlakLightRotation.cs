using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakLightRotation : MonoBehaviour
{
    [SerializeField] private Transform _lightCase = default;
    [SerializeField] private float _zRotationOffset = -90f;

    [SerializeField] private Light _light;
    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _lightCase.rotation.eulerAngles.z + _zRotationOffset);
    }
}