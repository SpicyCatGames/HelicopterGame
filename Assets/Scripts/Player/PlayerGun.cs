using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//targets can be identified by if they have <ITakeDamagable>
//but it's good to have seperate layers for enemy too for raycast, doing getcomponent on so many is bad for performance
//there will be two states, searching and locked/tracking
public class PlayerGun : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayers = default;
    [Header("SWEEP RAYCAST STUFF", order = 0)]
    [Space(20, order = 1)]
    [SerializeField] private Vector2 _firePoint = default;
    [SerializeField] private float _lowerAssistMaxAngle = 10f;
    [SerializeField] private float _higherAssistMaxAngle = 10f;
    [SerializeField] private int _sweepCastSections = 5;
    [SerializeField] private int _sectionsPerFrame = 1;
    [SerializeField] private float _rayCastDistance = 5f;

    private float _startAngle; //the euler Z at the minimum allowed z rotation
    private float _endAngle; //the euler Z at the max allowed z rotation
    private float _sectionWidth; //how many degrees the difference between each section
    private int _currentSectionIndex = 0;//the index of the section are we raycasting on the current frame 

    private enum _states
    {
        //scanning when sweepcasting for targets, otherwise tracking the single targets to see if there is a clear line of sight, and also must be in allowed angle
        scanning, tracking
    }
    private _states _currentState = _states.scanning;

    private Transform _target;
    //[SerializeField] private GameObject _round = default;
    //[SerializeField] private float _fireDelay = 1f;
    //[SerializeField] private float _firingAngleOffset = -90f;
    //private float _fireDelayCounter = 0;

    private void Start()
    {
        _startAngle = transform.rotation.eulerAngles.z - _lowerAssistMaxAngle;
        _endAngle = transform.rotation.eulerAngles.z + _higherAssistMaxAngle;
        _sectionWidth = (_endAngle - _startAngle) / _sweepCastSections; //total width divided by width per section
    }

    private void Update()
    {
        if (_currentState == _states.scanning) SweepRayCast();
    }

    private void SweepRayCast()
    {
        for (; _currentSectionIndex < _currentSectionIndex + _sectionsPerFrame; _currentSectionIndex++)
        {
            if (_currentSectionIndex >= _sweepCastSections)
            {
                _currentSectionIndex = 0;
                break;
            }
            float currentAngle = _startAngle + (_sectionWidth * _currentSectionIndex);
            Vector2 currentDirection = SilverUtils.Angle.Degrees.DegtoVec2(currentAngle);
            //raycast code here
            RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(_firePoint), currentDirection, _rayCastDistance, _targetLayers);
            if (hit.transform != null)
            {
                _target = hit.transform;
                _currentState = _states.tracking; //switch to tracking state when we hit something
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Draw firepoint
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(_firePoint), 0.2f);
        //draw sweep raycasts
        Gizmos.color = Color.cyan;
        //transform.rotation.z at the start will be the _startAngle, to run  this in playmode, simply change it with _startAngle
        float startRotation = transform.rotation.z - _lowerAssistMaxAngle;
        float endRotation = transform.rotation.z + _higherAssistMaxAngle;
        float sectionWidth = (endRotation - startRotation) / _sweepCastSections;
        for (float currentRotation = startRotation; currentRotation <= endRotation; currentRotation += sectionWidth)
        {
            //this will iterate between all the angles at which we need to raycast
            //for the actual game, we would do only one per frame
            //or have a way to adjust how many to do per frame which may become necessary
            //it can be dependent on player's speed too altho I donno what effect that will have on FPS
            //we can have a lock on to current target as long as in line of sight mechanism
            Gizmos.DrawLine(transform.TransformPoint(_firePoint), (Vector2)transform.TransformPoint(_firePoint) + (SilverUtils.Angle.Degrees.DegtoVec2(currentRotation) * _rayCastDistance));
        }
    }

    /*private void Update()
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
    }*/
}