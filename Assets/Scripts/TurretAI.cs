using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [SerializeField] private float rotationOffset;
    [SerializeField] private Transform target = default;
    [SerializeField][Range(0, 10)] private float firingRadius = 5;
    [SerializeField] private Vector2 rotationMin;
    [SerializeField] private Vector2 rotationMax;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject rounds;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float firingRotationOffset;
    [SerializeField] private float fireDelay = 1f;
    private float fireDelayDefault;
    private void Start()
    {
        fireDelayDefault = fireDelay;
    }
    private void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position);
        if (distance < firingRadius)
        {
            Vector2 _direction = target.position - transform.position;
            float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;//goes from 180 to -180
            if (_angle < 0)
            {
                _angle = 360f + _angle;
            }
            _angle = Mathf.Clamp(_angle, Vector2.Angle(transform.position, rotationMin), Vector2.Angle(transform.position, rotationMax));

            fireDelay -= Time.deltaTime;
            if (fireDelay < 0)
            {
                Instantiate(rounds, firePoint.position, Quaternion.AngleAxis(_angle - 90f + firingRotationOffset, Vector3.forward));
                fireDelay = fireDelayDefault;
            }

            _angle = _angle - 90f + rotationOffset;
            //transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            Quaternion rotationTemp = Quaternion.AngleAxis(_angle, Vector3.forward);
            if (rotationTemp.eulerAngles.z > transform.rotation.eulerAngles.z)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + (rotationSpeed * Time.deltaTime));
            }
            else if (rotationTemp.eulerAngles.z < transform.rotation.eulerAngles.z)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - (rotationSpeed * Time.deltaTime));
            }

        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firingRadius);
        Gizmos.DrawLine(transform.position, transform.TransformPoint(rotationMin));
        Gizmos.DrawLine(transform.position, transform.TransformPoint(rotationMax));
    }
}
