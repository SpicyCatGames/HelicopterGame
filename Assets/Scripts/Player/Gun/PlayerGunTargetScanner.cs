using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilverUtils.Angle;

//targets can be identified by if they have <ITakeDamagable>
//but it's good to have seperate layers for enemy too for raycast, doing getcomponent on so many is bad for performance
//there will be two states, searching and locked/tracking
public class PlayerGunTargetScanner : MonoBehaviour
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

    [SerializeField]Transform _parentTransform = default;

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

    public Transform _target { get; private set; }
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
        if (_currentState == _states.scanning)
        {
            SweepRayCast();
            //Debug.Log("Sweeping");
        }
        else
        {
            //Debug.Log($"Tracking {_target.name}");
            TrackTarget();
        }
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
            Vector2 currentDirection = _parentTransform.TransformDirection(SilverUtils.Angle.Degrees.DegtoVec2(currentAngle + 180));
            //raycast code here
            RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(_firePoint), currentDirection, _rayCastDistance, _targetLayers);
            if (hit.transform != null)
            {
                //now we only switch state if we have a clear line of sight to this hit
                RaycastHit2D sightCheck = Physics2D.Raycast(transform.TransformPoint(_firePoint), currentDirection, _rayCastDistance);
                if (sightCheck.transform == hit.transform) {
                    _target = hit.transform;
                    _currentState = _states.tracking; //switch to tracking state when we hit something
                    return;
                }
            }
        }
    }

    private void TrackTarget()
    {
        //track to see if _target is in line of sight and in given angle constraint
        //otherwise switch back to scanning state
        Vector2 direction = _target.position - transform.TransformPoint(_firePoint);
        //need to check here if it's between the constraints
        float directionEuler = Degrees.Normalizeto360(Degrees.Vec2toDeg(direction) + 90);
        Debug.Log(directionEuler + " " + _startAngle + " " + _endAngle);
        bool directionIsInConstraint = Degrees.RotationIsBetween(directionEuler, Degrees.Normalizeto360(_startAngle), Degrees.Normalizeto360(_endAngle));
        RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(_firePoint), direction, _rayCastDistance);
        if (hit.transform != _target || !directionIsInConstraint) //if not in line of sight or in constraint angles
        {
            _target = null;
            _currentState = _states.scanning;
        }
    }

    private void OnDrawGizmosSelected()
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
        for (int currentIndex = 0; currentIndex < _sweepCastSections; currentIndex++)
        {
            float currentRotation = startRotation + currentIndex * sectionWidth;
            //this will iterate between all the angles at which we need to raycast
            //for the actual game, we would do only one per frame
            //or have a way to adjust how many to do per frame which may become necessary
            //it can be dependent on player's speed too altho I donno what effect that will have on FPS
            //we can have a lock on to current target as long as in line of sight mechanism
            Gizmos.DrawLine(transform.TransformPoint(_firePoint), (Vector2)transform.TransformPoint(_firePoint) + (Vector2)_parentTransform.TransformDirection(SilverUtils.Angle.Degrees.DegtoVec2(currentRotation + 180) * _rayCastDistance));
        }
    }
}