using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAI : MonoBehaviour
{
    //this script would work well with a large range
    [SerializeField] private Transform _target = default;
    [SerializeField] private GameObject _round = default;
    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private float _maxLeftWalkDistance = 2f;
    [SerializeField] private float _shootLeftAngleOffset = 0;
    [SerializeField] private float _shootUpAngleOffset = 0;
    private float initialX;
    //Endpoints also used as firepoint
    [SerializeField] private Vector2 _leftRaycastDirectionBegin = new Vector2(-0.13f, 0.185f);
    [SerializeField] private Vector2 _leftRaycastDirectionEnd = new Vector2(-0.43f, 0.185f);
    [SerializeField] private Vector2 _upRaycastDirectionBegin = new Vector2(0.12f, 0.4f);
    [SerializeField] private Vector2 _upRaycastDirectionEnd = new Vector2(-0.14f, 0.61f);

    private Animator _anim;

    private enum SoldierStates
    {
        Idle, WalkLeft, ShootLeft, ShootUp
    }
    SoldierStates _currentState = SoldierStates.Idle;

    private void Start()
    {
        initialX = transform.position.x;

        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        SetStates();
        SetAnimatorVariable();
        States();
        
    }

    private void SetStates()
    {
        //setting the states
        if (Vector2.Distance(transform.position, _target.position) < _detectionRadius)//if target is in range
        {
            //stand, walk left, shoot straight, shoot up
            //raycasting left and up
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.TransformPoint(_leftRaycastDirectionBegin), transform.TransformPoint(_leftRaycastDirectionEnd) - transform.TransformPoint(_leftRaycastDirectionBegin), _detectionRadius);
            RaycastHit2D hitUp = Physics2D.Raycast(transform.TransformPoint(_upRaycastDirectionBegin), transform.TransformPoint(_upRaycastDirectionEnd) - transform.TransformPoint(_upRaycastDirectionBegin), _detectionRadius);
            if (initialX - _maxLeftWalkDistance < transform.position.x) //if we haven't crossed the _maxLeftWalkDistance
            {
                //shoot if ray finds target, otherwise walk
                if (hitLeft.transform == _target) _currentState = SoldierStates.ShootLeft;
                else if (hitUp.transform == _target) _currentState = SoldierStates.ShootUp;
                else _currentState = SoldierStates.WalkLeft;
            }
            else//if we have reached the _maxLeftWalkDistance
            {
                //shoot if ray finds target, otherwise default
                if (hitLeft.transform == _target) _currentState = SoldierStates.ShootLeft;
                else if (hitUp.transform == _target) _currentState = SoldierStates.ShootUp;
                else _currentState = SoldierStates.Idle;
            }
        }
        else//if target isn't in range
        {
            _currentState = SoldierStates.Idle;
        }
    }

    private void States()
    {
        //Doing state things and setting animator variables
        switch (_currentState)
        {
            case SoldierStates.Idle:

                break;
            case SoldierStates.WalkLeft:
                transform.position = new Vector2(transform.position.x - (_walkSpeed * Time.deltaTime), transform.position.y);
                break;
            case SoldierStates.ShootLeft:
                //setAnimator variable and use animevent to call fireleft method
                break;
            case SoldierStates.ShootUp:
                //setAnimator variable and use animevent to call fireUp method
                break;
        }
    }

    private void SetAnimatorVariable()
    {
        //Debug.Log(_currentState.ToString());
        if (_currentState.ToString() == "Idle") _anim.SetBool("Idle", true);
        else _anim.SetBool("Idle", false);
        if (_currentState.ToString() == "WalkLeft") _anim.SetBool("WalkLeft", true);
        else _anim.SetBool("WalkLeft", false);
        if (_currentState.ToString() == "ShootLeft") _anim.SetBool("ShootLeft", true);
        else _anim.SetBool("ShootLeft", false);
        if (_currentState.ToString() == "ShootUp") _anim.SetBool("ShootUp", true);
        else _anim.SetBool("ShootUp", false);

    }

    private void FireLeft()
    {
        Instantiate(_round, transform.TransformPoint(_leftRaycastDirectionEnd), Quaternion.AngleAxis(transform.rotation.z + _shootLeftAngleOffset, Vector3.forward));
    }
    private void FireUp()
    {
        Instantiate(_round, transform.TransformPoint(_upRaycastDirectionEnd), Quaternion.AngleAxis(transform.rotation.z + _shootUpAngleOffset, Vector3.forward));
    }

    private void OnDrawGizmosSelected()
    {
        //Range Circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        //Left walk distance
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.TransformPoint(new Vector2(-_maxLeftWalkDistance, 1)), transform.TransformPoint(new Vector2(-_maxLeftWalkDistance, -1)));
        //Raycast Lines
        Gizmos.color = Color.green;
        //for shooting left
        Gizmos.DrawLine(transform.TransformPoint(_leftRaycastDirectionBegin), transform.TransformPoint(_leftRaycastDirectionEnd));
        //for shooting up
        Gizmos.DrawLine(transform.TransformPoint(_upRaycastDirectionBegin), transform.TransformPoint(_upRaycastDirectionEnd));
    }
}