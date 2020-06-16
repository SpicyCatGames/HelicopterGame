using System;
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

        #region TouchInput() fields
        private TouchInput _touchInput;
        #endregion

        #region Editor only
#if UNITY_EDITOR
        enum EditorInputMethod {
            Keyboard, Touch
        }
        [SerializeField] private EditorInputMethod _editorInputMethod = EditorInputMethod.Keyboard;
#endif
        #endregion

        private void Start()
        {
            _touchInput = GetComponent<TouchInput>();
        }

        void Update()
        {
            #if UNITY_STANDALONE && !UNITY_EDITOR
                KeyboardInput();
            #endif
            #if UNITY_ANDROID && !UNITY_EDITOR
                TouchInput();
            #endif
            #region Editor only
            #if UNITY_EDITOR
                if (_editorInputMethod == EditorInputMethod.Keyboard)
                {
                    KeyboardInput();
                }
                else if (_editorInputMethod == EditorInputMethod.Touch)
                {
                    TouchInput();
                }
            #endif
            #endregion
        }

        void KeyboardInput()
        {
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");
            SpaceKey = Input.GetKey(KeyCode.Space);
        }

        void TouchInput()
        {
            if (_touchInput == null) return;
            Horizontal = _touchInput.ButtonInputValues.x;
            Vertical = _touchInput.ButtonInputValues.y;

            //fire/"SpaceKey" button, let's do that here and keep TouchInput class strictly for touch joystick
            SpaceKey = false;//resetting it before checking for input
            for (int x = 0; x < Input.touchCount; x++)
            {
                Touch touch = Input.GetTouch(x);
                if (!_touchInput.Controlling || touch.fingerId != _touchInput.controlFingerID) //making sure we do not use the controlling finger for jump
                {
                    if (touch.position.x > Screen.width/2)
                    {
                        SpaceKey = true;
                    }
                }
            }
        }
    }
}
