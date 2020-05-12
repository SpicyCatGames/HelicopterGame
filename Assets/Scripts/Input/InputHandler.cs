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

        void Update()
        {
            KeyboardInput();
        }

        void KeyboardInput()
        {
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");
            SpaceKey = Input.GetKey(KeyCode.Space);
        }
    }
}
