using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, ITakeDamagable
{
    [SerializeField] private float _health;
    public float Health => _health;

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }
}