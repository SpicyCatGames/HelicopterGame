using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, ITakeDamagable
{
    [SerializeField] private int _health;
    public int Health => _health;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {

    }
}