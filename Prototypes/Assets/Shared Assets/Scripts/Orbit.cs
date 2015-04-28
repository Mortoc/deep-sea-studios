using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
	public class Orbit : MonoBehaviour
	{
        [SerializeField]
        private Transform _center;

        [SerializeField]
        private float _speed;

        private Vector3 _initialOffset;

        void LateUpdate()
        {
            transform.RotateAround(_center.position, _center.up, _speed * Time.deltaTime);
        }
	}
}
