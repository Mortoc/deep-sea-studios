using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    [RequireComponent(typeof(TBOrbit))]
    public class CameraWander : MonoBehaviour
    {
        [SerializeField]
        private float _tiltStrength = 0.05f;

        [SerializeField]
        private float _tiltSpeed = 0.25f;

        private TBOrbit _orbit;

        void Start()
        {
            _orbit = GetComponent<TBOrbit>();
        }

        void Update()
        {
            var timeSpeed = Time.time * _tiltSpeed;
            var yawModifier = Mathf.Lerp(-1.0f, 1.0f, Mathf.PerlinNoise(timeSpeed, 0.0f)) * _tiltStrength;
            var pitchModifier = Mathf.Lerp(-1.0f, 1.0f, Mathf.PerlinNoise(0.0f, timeSpeed)) * _tiltStrength;
            
            _orbit.IdealPitch = _orbit.IdealPitch + yawModifier;
            _orbit.IdealYaw = _orbit.IdealYaw + yawModifier;
        }
    }
}
