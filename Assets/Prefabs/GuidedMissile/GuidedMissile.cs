using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GuidedMissile : MonoBehaviour
{
    public Transform _target = default; //make sure the laucher turret or whatever sets this at launch
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _rotatingSpeed = 5f;
    [SerializeField] private float lifeTime = 10f;
    private Rigidbody2D _rb;

	void Start()
	{
        _rb = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0) Destroy(this.gameObject);
    }

    void FixedUpdate()
	{
        Vector2 direction = _target.position - transform.position;
        
        float rotateAmount = Vector3.Cross(direction.normalized, transform.up).z;

        float angle = Vector2.Dot(direction, transform.up);//to make sure missile doesn't follow when it's past you and only when you are in front

        if (angle >= 0) {//only follow if player is in front
            _rb.angularVelocity = -rotateAmount * _rotatingSpeed;
        }
        else
        {
            _rb.angularVelocity = 0;
        }
        _rb.velocity = transform.up * _speed;
	}
}