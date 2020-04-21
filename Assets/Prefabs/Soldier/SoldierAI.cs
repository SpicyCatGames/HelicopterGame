using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAI : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private Transform _target = default;

	void Update()
	{
        if (Vector2.Distance(transform.position, _target.position) < _detectionRadius)
        {
            //walk left and raycast, shoot if you detect target in line of sight
            //stand, walk left, shoot straight, shoot up
        }
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}