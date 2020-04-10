using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private customInputs.InputHandler myInputs = default;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float maxRotation = 30f;
    [SerializeField][Range(0 , 1)] private float minRotationtoStabilize = .3f;
    [SerializeField] private float stabilizationSpeed = 1f;
    [SerializeField] private float horizontalSpeed = 3f;
    [SerializeField] private float verticalSpeed = 0.7f;
    [SerializeField] private float descendingMultiplier = 1.4f;

    private Rigidbody2D _rb;
    private float initialRotation;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation.eulerAngles.z;
    }
    void Update()
    {
        ClampRotation();
        RotatePlayer();
        Movement();
    }

    private void RotatePlayer()
    {
        //rotate
        _rb.AddTorque( -myInputs.Horizontal * rotationSpeed);

        //Stabilize
        float rotationZ = From360To180(transform.eulerAngles.z);
        float stabilizationMultiplier = rotationZ / maxRotation;
        if (Mathf.Abs(stabilizationMultiplier) > minRotationtoStabilize)
        { 
            stabilizationMultiplier = (stabilizationMultiplier - minRotationtoStabilize) / (1-minRotationtoStabilize);//change the range from minRotationtoStabilize >> 1 to 0 >> 1
            _rb.AddTorque(stabilizationMultiplier * stabilizationSpeed);
        }

        //Fix max rotation Hang
        if (Mathf.Abs(rotationZ) >= (maxRotation - 1) && !Mathf.Approximately(_rb.angularVelocity, 0)) //if rotation is too close to maxRotation and rotating
        {
            if ((rotationZ < 0 && _rb.angularVelocity > 0) || (rotationZ > 0 && _rb.angularVelocity < 0)) //if near max rotation and still trying to rotate to that direction
            {
                _rb.angularVelocity = -.1f;
            }
        }
    }
    private void ClampRotation()
    {
        float zClamped = From360To180(transform.eulerAngles.z);
        zClamped = Mathf.Clamp(zClamped, initialRotation - maxRotation, initialRotation + maxRotation);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zClamped);
    }
    private void Movement()
    {
        float horizontalMovement = Mathf.Sin(From360To180(transform.eulerAngles.z) / maxRotation * 90 * Mathf.Deg2Rad) * horizontalSpeed;
        float verticalMovement = myInputs.Vertical;

        verticalMovement *= verticalSpeed;
        if(myInputs.Vertical < 0) //if going down
        {
            verticalMovement *= descendingMultiplier;
        }

        _rb.AddForce(new Vector2(horizontalMovement, verticalMovement));
    }

    private float From360To180(float rotation)
    {
        return rotation = (rotation > 180) ? (-360 + rotation) : rotation;
    }
}
