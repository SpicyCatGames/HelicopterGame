using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace customInputs
{

    public class InputHandler : MonoBehaviour
    {
        private float horizontal;
        public float Horizontal { get => horizontal; }
        private float vertical;
        public float Vertical { get => vertical; }
        void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
    }



}
