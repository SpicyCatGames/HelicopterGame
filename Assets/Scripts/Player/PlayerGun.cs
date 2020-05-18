using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private GameObject _round = default;
    [SerializeField] private Vector2 _firePoint = default;
    [SerializeField] private float _fireDelay = 1f;
    [SerializeField] private float _firingAngleOffset = -90f;
    private float _fireDelayCounter = 0;


    private void Update()
    {
        if (customInputs.InputHandler.SpaceKey && _fireDelayCounter < 0)
        {
            GameObject launchedRound = Instantiate(_round, transform.TransformPoint(_firePoint), Quaternion.AngleAxis(_firingAngleOffset - transform.eulerAngles.z, Vector3.forward));
            Shell shellScript = launchedRound.GetComponent<Shell>();
            shellScript._velocityRB = GetComponent<Rigidbody2D>();
            shellScript._tagToIgnore = tag;
            _fireDelayCounter = _fireDelay;
        }
        _fireDelayCounter -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(_firePoint), 0.1f);
    }
}