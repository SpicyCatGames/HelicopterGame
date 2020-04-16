using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GuidedMissile : MonoBehaviour
{
    public Transform _target = default; //make sure the laucher turret or whatever sets this at launch
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _rotatingSpeed = 5f;
    [SerializeField] private float _lifeTime = 10f;
    [SerializeField] private float delayToDamageLauncher = 0.5f; //the amount of time after which it will be able to damage the device it was launched from
    //without delayToDamageLauncher, it sometimes hurts the launcher when launching
    [SerializeField] private string launcherTag = default;
    private Rigidbody2D _rb;
    [Header("Stats")]
    [SerializeField] private int _damage = 40;
    [SerializeField] private GameObject _explosion = default;
    [SerializeField] private float _aoeRadius = 2f;

    private float lifeTimeDefault;

	void Start()
	{
        _rb = GetComponent<Rigidbody2D>();
        lifeTimeDefault = _lifeTime;
	}

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime < 0) Destroy(this.gameObject);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (launcherTag != null && collision.transform.tag == launcherTag) { //if colliders with launcher that lauched this missile
            if (_lifeTime + delayToDamageLauncher < lifeTimeDefault)//if activationdelay has passed
            {
                Destroy(gameObject);
            }
        }
        else//if colliding with other object
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, _aoeRadius); //aoe check
        foreach (Collider2D hitObject in hitObjects) //aoe damage
        {
            hitObject.GetComponent<ITakeDamagable>()?.TakeDamage(_damage);
        }

        Instantiate(_explosion, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _aoeRadius);
    }
}