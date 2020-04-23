using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakLightRotator : MonoBehaviour
{
    [SerializeField] private float _rotatingSpeed = 1f;
    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + _rotatingSpeed * Time.deltaTime, transform.rotation.eulerAngles.z);
    }
}