using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStats : MonoBehaviour, ITakeDamagable
{
    [SerializeField] private int _health = 10;
    public int Health => _health;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0) Destroy(gameObject);
    }
}