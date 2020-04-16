using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{
    [SerializeField] private float _speed = 2;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _lifeTime = 10;
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
        collision.GetComponent<ITakeDamagable>()?.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
