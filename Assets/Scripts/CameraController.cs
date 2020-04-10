using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField][Range(0, 1)]private float smoothTime;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), smoothTime);
    }
}
