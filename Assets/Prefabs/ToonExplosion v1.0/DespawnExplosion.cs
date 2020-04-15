﻿using UnityEngine;

public class DespawnExplosion : MonoBehaviour
{
    [SerializeField]private float _lifeTime = 1.0f;
    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0) Destroy(gameObject);
    }
}