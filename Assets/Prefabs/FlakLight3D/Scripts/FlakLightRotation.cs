using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
//for this I had to change the light2D script, original code in the comment of that script. more info: 
//https://stackoverflow.com/questions/58120976/how-can-i-access-the-unity-light-2d-script-component-via-script
//enable Executealways when adjusting relativeposition
//[ExecuteAlways]
public class FlakLightRotation : MonoBehaviour
{
    [SerializeField] private Transform _lightCase = default;
    [SerializeField] private Vector3 _relativePosition = default;
    [SerializeField] private float _zRotationOffset = -90f;

    [SerializeField] private Transform _rotatingParent = default;
    public Light2D _fllakLight2D;
    private float initialInnerRadius;
    private float initialOuterRadius;
    [SerializeField] [Range(0, 1)] float _minLightRadius = 0.8f;

    private void Start()
    {
        initialInnerRadius = _fllakLight2D.pointLightInnerRadius;
        initialOuterRadius = _fllakLight2D.pointLightOuterRadius;
    }

    //Rotation of light is expected to move between 90 > 0 > 270 on the z
    //scalebetween rotation and 180-rotation
    //Lightcase.z moves 180 > 359(>0)
    private void Update()
    {
        //Calculate light properties
        
        //>180 if looking at the player, <180 if looking into the screen
        float angleOfLight = _rotatingParent.eulerAngles.y;
        bool isLookingIntoScreen = (angleOfLight < 180) ? true : false;
        //0 -> 360 to 0 > 180 > 0 if it's on the right side, it's >90 else <90
        if (angleOfLight > 180)
        {
            angleOfLight = 360 - angleOfLight;
        }
        float angleBuffer = angleOfLight;//for angle z calculation of this light
        if (angleOfLight > 90) angleOfLight = 180 - angleOfLight; //0 > 180 > 0 to 0 > 90 > 0 > 90 > 0
        
        float normalizedRadiusofLight = 1 - (angleOfLight / 90);//scaling between 1 -> 0
        normalizedRadiusofLight = (normalizedRadiusofLight * (1 - _minLightRadius)) + _minLightRadius; //scaling from minradius to 1

        //Assign light properties
        _fllakLight2D.pointLightInnerRadius = initialInnerRadius * normalizedRadiusofLight;
        _fllakLight2D.pointLightOuterRadius = initialOuterRadius * normalizedRadiusofLight;

        //scale anglebuffer from rotation to 180-rotation
        float tempCaseZ = _lightCase.rotation.eulerAngles.z - 270;//90 to 0 to -90
        //0 > 180 
        if (tempCaseZ >= 0) tempCaseZ = 90 - tempCaseZ;
        else tempCaseZ = -tempCaseZ + 90;
        float scaleTo = (90 - tempCaseZ) * 2; //scale the rotation of rotatingpart form 0 to this
        float YAdjustmentAngle = (angleBuffer * scaleTo) / 180;

        transform.position = _lightCase.transform.position + _relativePosition;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _lightCase.rotation.eulerAngles.z + _zRotationOffset - YAdjustmentAngle);
        //negate the scaled rotation from above line
    }
}