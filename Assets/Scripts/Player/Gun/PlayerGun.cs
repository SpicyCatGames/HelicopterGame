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
    [SerializeField]private Transform _parentTransform = default;

    [SerializeField] private float _rotationSpeed = 2f;

    private void Start()
    {
        _targetScanner = GetComponent<PlayerGunTargetScanner>();
        _initialRotationZ = transform.localEulerAngles.z;
        _transform = transform;
    }

    void Update()
	{
        Transform _target = _targetScanner._target;
        if (_target != null)
        {
            Vector2 direction = _target.position - _transform.position;
            float directionEuler = Degrees.Vec2toDeg(direction);
            _transform.eulerAngles = 
               new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, Mathf.MoveTowardsAngle(_transform.eulerAngles.z, -directionEuler, _rotationSpeed * Time.deltaTime));
            Debug.Log(directionEuler);
        }
        else
        {
            _transform.localEulerAngles = 
                new Vector3(_transform.localEulerAngles.x, _transform.localEulerAngles.y, Mathf.MoveTowardsAngle(_transform.localEulerAngles.z, _initialRotationZ, _rotationSpeed * Time.deltaTime));
        }
	}
}