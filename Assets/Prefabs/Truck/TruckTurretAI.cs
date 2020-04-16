﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilverUtils.Angle;

public class TruckTurretAI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform rotatingPart = default;
    [SerializeField] private Transform target = default;
    [Header("RotationThings")]
    [SerializeField] private float lookAngleOffset = -90f;
    [SerializeField] [Range(0, 5)] private float targetingDeadzone = 2f; //how many degrees the target has to move before turret moves
    [SerializeField] private bool limitToOneDirection = true; //only moves when target on left or right, disabling this means the facingLeft variable becomes useless
    [SerializeField] private bool facingLeft = true; //limit targeting to left or right side
    [Header("ClampRotationCounterClockWise")]
    [SerializeField] [Range(0, 359)] private float minRotation = 0;
    [SerializeField] [Range(0, 359)] private float maxRotation = 359;
    [Header("Stats")]
    [SerializeField] private float rotatingSpeed = 20f;
    [SerializeField] private float firingRange = 3f;
    [Header("Rounds")]
    [SerializeField] private GameObject round = default;
    [SerializeField] private Vector2 firePoint = default;
    [SerializeField] private Vector2 laserDirection = default;
    [SerializeField] private float lockOnDelay = 2f;
    private float fireDelayTemp = 0;
    [SerializeField] [Range(0, 359)] private float firingAngleOffset = 0;
    [Header("Opposite Lock Fix")]
    [SerializeField] private bool Fix1 = false;
    [SerializeField] private bool Fix2 = false;

    private LineRenderer laserLR = default;
    private GameObject launchedMissile = null;

    private void Start()
    {
        laserLR = GetComponent<LineRenderer>();
        //laserLR.enabled = false;
    }

    private void Update()
    {
        if (Vector2.Distance(rotatingPart.position, target.position) < firingRange)
        { //if target in range
            LookAtTarget();
            if (launchedMissile == null)
            {
                Fire();
            }
            else laserLR.enabled = false; //disble laser immediately if we launched a missile
        }
        else
        {
            StartCoroutine(DisableLR());
        }
    }

    private void LookAtTarget()
    {
        Vector2 direction = target.transform.position - rotatingPart.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + lookAngleOffset;
        if (angle < 0) angle += 360; //-180 -> 180 to 0 -> 360

        int rotatingDirection = RotatingDirection(rotatingPart.rotation.eulerAngles.z, angle);

        rotatingPart.rotation = Quaternion.AngleAxis(rotatingPart.rotation.eulerAngles.z + (rotatingDirection * rotatingSpeed * Time.deltaTime), Vector3.forward);
        //rotatingPart.rotation = Quaternion.Euler(rotatingPart.rotation.eulerAngles.x, rotatingPart.rotation.eulerAngles.y, rotatingPart.rotation.eulerAngles.z + (rotatingDirection * rotatingSpeed * Time.deltaTime)); //does the same thing
    }

    private int RotatingDirection(float currentRotation, float targetRotation)
    {
        //if target not on the correct side, don't move
        if (!targetOnCorrectSide()) return 0;

        if (currentRotation <= 180 && targetRotation > 180)
        {
            if(!Fix1)targetRotation = Degrees.From360to180(targetRotation);
            minRotation = Degrees.From360to180(minRotation);
        }
        else if (targetRotation <= 180 && currentRotation > 180)
        {
            if(!Fix2)currentRotation = Degrees.From360to180(currentRotation);
            maxRotation = Degrees.From360to180(maxRotation);
        }
        //the first lines of these two if statements are to avoid snapping at the edges because of rotations going beyond zero or 359
        //the second one is for the clamp lines up next

        if ((currentRotation < minRotation && targetRotation < minRotation) || (currentRotation > maxRotation && targetRotation > maxRotation))
        {
            return 0;
        }//clamp

        if (Mathf.Abs(currentRotation - targetRotation) > targetingDeadzone)
        {
            if (currentRotation < targetRotation) return 1;
            else if (currentRotation > targetRotation) return -1;
        }
        return 0;
    }

    private void Fire()
    {
        Vector2 _raycastDirection = rotatingPart.TransformPoint(laserDirection) - rotatingPart.TransformPoint(firePoint);
        RaycastHit2D hitObject = Physics2D.Raycast(rotatingPart.TransformPoint(firePoint), _raycastDirection, firingRange);

        if (hitObject.transform != null && hitObject.transform == target)
        { // Target sighted
            fireDelayTemp -= Time.deltaTime;
            //line renderer
            laserLR.enabled = true;
            laserLR.SetPosition(0, rotatingPart.TransformPoint(firePoint));
            laserLR.SetPosition(1, hitObject.point);
            if (fireDelayTemp < 0 && targetOnCorrectSide())//if countdown has reached 0, fire a round
            {
                launchedMissile = Instantiate(round, rotatingPart.TransformPoint((Vector3)firePoint), Quaternion.AngleAxis(rotatingPart.eulerAngles.z + firingAngleOffset, Vector3.forward));
                launchedMissile.GetComponent<GuidedMissile>()._target = target;
                fireDelayTemp = lockOnDelay;
            }
        }
        else // Target lost
        {
            fireDelayTemp = lockOnDelay;
            StartCoroutine(DisableLR());
        }
    }

    private bool targetOnCorrectSide()
    {
        if (limitToOneDirection && ((facingLeft && Vector2.Dot(target.position - transform.position, Vector3.left) <= 0) || (!facingLeft && Vector2.Dot(target.position - transform.position, Vector3.left) >= 0)))
        {
            return false;
        }
        return true;
    }

    private IEnumerator DisableLR()
    {
        laserLR.SetPosition(0, rotatingPart.TransformPoint(firePoint));
        laserLR.SetPosition(1, rotatingPart.TransformPoint(laserDirection * 10));
        yield return new WaitForSeconds(1);
        laserLR.enabled = false;
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rotatingPart.position, firingRange);

        Gizmos.color = Color.cyan;
        float degMin = Mathf.Deg2Rad * (minRotation - lookAngleOffset);
        Gizmos.DrawLine(rotatingPart.position, (Vector2)rotatingPart.position + new Vector2(Mathf.Cos(degMin), Mathf.Sin(degMin)) * firingRange);
        Gizmos.color = Color.red;
        float degMax = Mathf.Deg2Rad * (maxRotation - lookAngleOffset);
        Gizmos.DrawLine(rotatingPart.position, (Vector2)rotatingPart.position + new Vector2(Mathf.Cos(degMax), Mathf.Sin(degMax)) * firingRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(rotatingPart.TransformPoint(firePoint), 0.1f);

        Gizmos.DrawLine(rotatingPart.TransformPoint(firePoint), rotatingPart.TransformPoint(laserDirection));
    }
}
