using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class FpsDisplay : MonoBehaviour
    {
        public Text _output;
        public float _updateRate = 1.0f;
        private float _lastUpdate = 0.0f;

        void Update()
        {
            if (_lastUpdate + _updateRate <= Time.time)
            {
                _output.text = (1.0f / Time.deltaTime).ToString("f1") + " fps";
                _lastUpdate = Time.time;
            }
        }
    }
}
