using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    Camera cam;

    public Vector2 _originViewport = new Vector2(.5f, .5f);
    private Vector2 originScreen = default;
    [SerializeField][Range(0, 1)] private float _sizeFromHeight = .3f;
    [SerializeField][Range(0, 1)] private float _buttonLowerDeadZone = .333f;
    
    public bool Controlling { get; private set; } = false;
    public int controlFingerID { get; private set; }
    public Vector2 InputValues { get; private set; } = default;
    public Vector2Int ButtonInputValues { get; private set; } = default; //this will be like getaxisraw, To be implemented
    public Vector2 PointerPosition { get; private set; } = default;
    public Vector2 PixelDelta { get; private set; } = default;
    public Vector2 UnitDelta { get; private set; } = default;

    #region Debug
    #if UNITY_EDITOR
    [Header("This should be disabled before play mode to avoid issues")]
    [SerializeField] private bool GizmosEnabledInSceneView = false;
    #endif
    #endregion

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        ProcessTouch();

        if (!Controlling)
        {
            ResetProperties();
        }
    }

    private void ResetProperties()
    {
        InputValues = default;
        PointerPosition = originScreen;
        PixelDelta = default;
        UnitDelta = default;
        ButtonInputValues = default;
    }

    private void ProcessTouch()
    {
        for (int x = 0; x < Input.touchCount; x++)
        {
            Touch touch = Input.GetTouch(x);
            originScreen = cam.ViewportToScreenPoint(_originViewport);
            float radiusInPixels = Screen.height * _sizeFromHeight / 2;

            if (Vector2.Distance(touch.position, originScreen) <= radiusInPixels && touch.phase == TouchPhase.Began && !Controlling)
            {
                controlFingerID = touch.fingerId;
                Controlling = true;
            }
            if (touch.fingerId == controlFingerID && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                Controlling = false;
            }

            #region Debug
            #if UNITY_EDITOR
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y, 10)), Color.red, 0, false);
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y - radiusInPixels, 10)), Color.red, 0, false);
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y + radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y + radiusInPixels, 10)), Color.red, 0, false);
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x, originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x, originScreen.y + radiusInPixels, 10)), Color.red, 0, false);
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y + radiusInPixels, 10)), Color.red, 0, false);
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y + radiusInPixels, 10)), Color.red, 0, false);
                //deadzone lines
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y - (radiusInPixels * _buttonLowerDeadZone), 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y - (radiusInPixels * _buttonLowerDeadZone), 10)), Color.red, 0, false);
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y + (radiusInPixels * _buttonLowerDeadZone), 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y + (radiusInPixels * _buttonLowerDeadZone), 10)), Color.red, 0, false);
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - (radiusInPixels * _buttonLowerDeadZone), originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x - (radiusInPixels * _buttonLowerDeadZone), originScreen.y + radiusInPixels, 10)), Color.red, 0, false);
                Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x + (radiusInPixels * _buttonLowerDeadZone), originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + (radiusInPixels * _buttonLowerDeadZone), originScreen.y + radiusInPixels, 10)), Color.red, 0, false);
            #endif
            #endregion
            if (touch.fingerId == controlFingerID && Controlling)
            {
                PixelDelta = touch.position - originScreen;
                InputValues = PixelDelta / radiusInPixels; //scale them from 0 at origin to 1 at circumference
                UnitDelta = PixelDelta * (Screen.height / 2) / Camera.main.orthographicSize; //pixelDelta * PPU

                InputValues = new Vector2(Mathf.Clamp(InputValues.x, -1, 1), Mathf.Clamp(InputValues.y, -1, 1));

                //making it so that values can only be -1, 0, 1 like buttons
                int inputX = (Mathf.Abs(InputValues.x) < _buttonLowerDeadZone) ? 0 : 1 * (int)Mathf.Sign(InputValues.x);
                int inputY = (Mathf.Abs(InputValues.y) < _buttonLowerDeadZone) ? 0 : 1 * (int)Mathf.Sign(InputValues.y);
                ButtonInputValues = new Vector2Int(inputX, inputY);

                PointerPosition = originScreen + InputValues * radiusInPixels;

                #region Debug
                #if UNITY_EDITOR
                    Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(PointerPosition.x, PointerPosition.y, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x, originScreen.y, 10)));
                #endif
                #endregion
            }
        }
    }

    #region Debug
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!GizmosEnabledInSceneView)
        {
            return;
        }
        cam = Camera.main;
        originScreen = cam.ViewportToScreenPoint(_originViewport);
        float radiusInPixels = Screen.height * _sizeFromHeight / 2;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y, 10)));
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y - radiusInPixels, 10)));
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y + radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y + radiusInPixels, 10)));
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x, originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x, originScreen.y + radiusInPixels, 10)));
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y + radiusInPixels, 10)));
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y + radiusInPixels, 10)));

        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y - (radiusInPixels * _buttonLowerDeadZone), 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y - (radiusInPixels * _buttonLowerDeadZone), 10)));
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - radiusInPixels, originScreen.y + (radiusInPixels * _buttonLowerDeadZone), 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + radiusInPixels, originScreen.y + (radiusInPixels * _buttonLowerDeadZone), 10)));
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x - (radiusInPixels * _buttonLowerDeadZone), originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x - (radiusInPixels * _buttonLowerDeadZone), originScreen.y + radiusInPixels, 10)));
        Gizmos.DrawLine(cam.ScreenToWorldPoint(new Vector3(originScreen.x + (radiusInPixels * _buttonLowerDeadZone), originScreen.y - radiusInPixels, 10)), cam.ScreenToWorldPoint(new Vector3(originScreen.x + (radiusInPixels * _buttonLowerDeadZone), originScreen.y + radiusInPixels, 10)));
    }
    #endif
    #endregion
}