using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour, ITakeDamagable
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
            //_health = 0;
            StartCoroutine(PlayerDeath());
        }
    }

    private IEnumerator PlayerDeath()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        if (_explosion != null) Instantiate(_explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        throw new NotImplementedException();
    }
}