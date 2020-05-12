using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace customInputs
{

    public class InputHandler : MonoBehaviour
    {
        public static float Horizontal { get; private set; }
        public static float Vertical { get; private set; }
        public static bool SpaceKey { get; private set; }

        #region TouchInput fields
        private Vector2 _startTouchPosition;
        [SerializeField] [Range(0, 1)] private float radiusSizeMultiplier = 1; //how much of the display height to be used as ui joystick radius
        #endregion

        void Update()
        {
            KeyboardInput();
            //MouseInput(); This is just an easy way to debug touch
            TouchInput();
        }

        void KeyboardInput()
        {
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");
            SpaceKey = Input.GetKey(KeyCode.Space);
        }

        void MouseInput()
        {
            float scalingDivisor = Screen.height * radiusSizeMultiplier;
            if (Input.GetMouseButtonDown(0)) //equivalent to touch start
            {
                _startTouchPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0)) //equivalent to touch continuing
            {
                float horizontalUnscaled = Input.mousePosition.x - _startTouchPosition.x;
                float horizontalScaled = horizontalUnscaled / scalingDivisor;
                Horizontal = (horizontalScaled < 1) ? horizontalScaled : 1;

                float verticalUnscaled = Input.mousePosition.y - _startTouchPosition.y;
                float verticalScaled = verticalUnscaled / scalingDivisor;
                Vertical = (verticalScaled < 1) ? verticalScaled : 1;
            }
            else if (Input.GetMouseButtonUp(0)) //equivalent to touch release
            {
                Horizontal = 0;
                Vertical = 0;
            }
        }

        void TouchInput()
        {
            if (Input.touchCount > 0)
            {
                float scalingDivisor = Screen.height * radiusSizeMultiplier;
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startTouchPosition = touch.position;
                        break;

                    case TouchPhase.Moved:
                        float horizontalUnscaled = touch.position.x - _startTouchPosition.x;
                        float horizontalScaled = horizontalUnscaled / scalingDivisor;
                        Horizontal = (horizontalScaled < 1) ? horizontalScaled : 1;

                        float verticalUnscaled = touch.position.y - _startTouchPosition.y;
                        float verticalScaled = verticalUnscaled / scalingDivisor;
                        Vertical = (verticalScaled < 1) ? verticalScaled : 1;
                        break;

                    case TouchPhase.Ended:
                        Horizontal = 0;
                        Vertical = 0;
                        break;
                }
            }
        }
    }
}
