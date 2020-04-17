using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{
    [SerializeField] private float _speed = 2;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _lifeTime = 10;
    [Header("Optional RB2D filled by launcher")]
    public Rigidbody2D _velocityRB = default; // to adjust velocity if launcher is moving 
    [SerializeField] private float _rbSpeedMultiplier = .1f;
    [Header("Optional,filled by launcher")]
    public string _tagToIgnore = default;
    private void Start()
    {
        if (_velocityRB != null)
        {
            //_speed += _velocityRB.velocity.x * _rbSpeedMultiplier;
            GetComponent<Rigidbody2D>().velocity = _velocityRB.velocity * _rbSpeedMultiplier;
        }
    }
    void FixedUpdate()
    {
        transform.position += transform.TransformDirection(Vector2.up) * _speed * Time.deltaTime;
    }
    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _tagToIgnore) return;
        collision.GetComponent<ITakeDamagable>()?.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
