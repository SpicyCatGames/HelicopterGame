using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilverUtils.Angle;

[RequireComponent(typeof(PlayerGunTargetScanner))]
public class PlayerGun : MonoBehaviour
{
    private PlayerGunTargetScanner _targetScanner;
    private float _initialRotationZ;
    private Transform _transform;

    [SerializeField] private float _rotationSpeed = 2f;

    [SerializeField] private GameObject _round = default;
    private Vector2 _firePoint;
    [SerializeField][Range(0, 270)] private float _firingAngleOffet = 0;
    [SerializeField] private float _fireDelay = .5f;
    private float _fireCountDown = 0;

    Transform _target;

    private void Start()
    {
        _targetScanner = GetComponent<PlayerGunTargetScanner>();
        _firePoint = _targetScanner._firePoint;
        _initialRotationZ = transform.localEulerAngles.z;
        _transform = transform;
    }

    void Update()
    {
        _target = _targetScanner._target;
        RotateGun();
        Fire();
    }

    private void RotateGun()
    {
        if (_target != null)
        {
            Vector2 direction = _target.position - _transform.position;
            float directionEuler = Degrees.Vec2toDeg(direction);
            _transform.eulerAngles =
               new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, Mathf.MoveTowardsAngle(_transform.eulerAngles.z, -directionEuler, _rotationSpeed * Time.deltaTime));
        }
        else
        {
            _transform.localEulerAngles =
                new Vector3(_transform.localEulerAngles.x, _transform.localEulerAngles.y, Mathf.MoveTowardsAngle(_transform.localEulerAngles.z, _initialRotationZ, _rotationSpeed * Time.deltaTime));
        }
    }

    private void Fire()
    {
        //if fire button isn't pressed
        if (customInputs.InputHandler.SpaceKey == false)
        {
            //this is so that delay isn't set to some number less and there is a delay in firing next time fire button is pressed
            if (_fireCountDown < _fireDelay)
            {
                _fireCountDown += Time.deltaTime;
            }
            return;
        }
        // This is if you want to only fire when target is sighted
        //if (_target == null)
        //{
        //    return;
        //}
        _fireCountDown += Time.deltaTime;
        if (_fireCountDown > _fireDelay)
        {
            Instantiate(_round, _transform.TransformPoint(_firePoint), Quaternion.AngleAxis(-_transform.eulerAngles.z + _firingAngleOffet, Vector3.forward));
            _fireCountDown = 0;
        }
    }
}