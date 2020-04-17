using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace customInputs
{

    public class InputHandler : MonoBehaviour
    {
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public bool SpaceKey { get; private set; }
        void Update()
        {
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");
            SpaceKey = Input.GetKey(KeyCode.Space);
        }
    }



}
