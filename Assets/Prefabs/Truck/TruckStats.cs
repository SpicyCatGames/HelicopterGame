using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckStats : MonoBehaviour, ITakeDamagable
{

    [SerializeField] private int _health = 100;
    public int Health => _health;
    [Header("Optional Explosion")]
    [SerializeField] private GameObject _explosion = default;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        if (_explosion != null) Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}