using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckTurretAI : MonoBehaviour
{
    [SerializeField] private float rotationOffset = -43.9f;
    [SerializeField] private Transform target = default;
    [SerializeField] [Range(0, 10)] private float firingRadius = 5;
    [SerializeField] private Vector2 rotationMin = default;
    [SerializeField] private Vector2 rotationMax = default;
    [SerializeField] private float rotationSpeed = 11f;
    [SerializeField] private GameObject rounds = default;
    [SerializeField] private Transform firePoint = default;
    [SerializeField] private float fireDelay = 1f;
    [SerializeField] private Vector2 _directionOfFire = default;
    private float fireDelayTemp;
    private LineRenderer _lr;

    private void Start()
    {
        fireDelayTemp = fireDelay;
        _lr = GetComponent<LineRenderer>();
        _lr.enabled = false;
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

            Fire();

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
        else// from the else of Fire()
        {
            StartCoroutine(DisbleLineRenderer());
        }
    }
    private void Fire()
    {
        RaycastHit2D lockedTarget =  Physics2D.Raycast(firePoint.position, transform.TransformPoint(_directionOfFire) - firePoint.position, firingRadius);
        Debug.Log(lockedTarget.transform != null);
        if (lockedTarget.transform != null && lockedTarget.transform.tag == "Player")
        {
            fireDelayTemp -= Time.deltaTime;
            _lr.enabled = true;
            _lr.SetPosition(0, firePoint.TransformPoint(new Vector3(0,0,0)));
            _lr.SetPosition(1, lockedTarget.point);
        }
        else
        {
            StartCoroutine(DisbleLineRenderer());
            
            fireDelayTemp = fireDelay;
        }
        
        if (fireDelayTemp < 0)
        {
            Instantiate(rounds, firePoint.position, Quaternion.AngleAxis(firePoint.rotation.eulerAngles.z + 90f, Vector3.forward));
            fireDelayTemp = fireDelay;
        }
    }

    private IEnumerator DisbleLineRenderer()
    {
        _lr.SetPosition(1, firePoint.TransformPoint((Vector3)_directionOfFire * 3));
        yield return new WaitForSeconds(0.5f);
        _lr.enabled = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firingRadius);
        Gizmos.DrawLine(transform.position, transform.TransformPoint(rotationMin));
        Gizmos.DrawLine(transform.position, transform.TransformPoint(rotationMax));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(firePoint.position, transform.TransformPoint((Vector3)_directionOfFire));
    }
}
