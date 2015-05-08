using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class LinearActuator : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _end1;

        [SerializeField]
        private Rigidbody _end2;

        [SerializeField]
        private Rigidbody _center;

        [SerializeField]
        private ConstantForce _expandForce1;

        [SerializeField]
        private ConstantForce _expandForce2;

        [SerializeField]
        private float _expandPower = 100.0f;

        [SerializeField]
        private Plug _plug;

        private void IgnoreAllCollisions(IList<Collider> colliders)
        {
            for (var i = 0; i < colliders.Count; ++i)
            {
                for (var j = i + 1; j < colliders.Count; ++j)
                {
                    Physics.IgnoreCollision(colliders[i], colliders[j]);
                }
            }
        }

        void Awake()
        {
            IgnoreAllCollisions(GetComponentsInChildren<Collider>());
        }

        void OnEnable()
        {
            _plug.OnPower += Expand;
            _plug.OnPowerLoss += Contract;
        }

        void OnDisable()
        {
            _plug.OnPower -= Expand;
            _plug.OnPowerLoss -= Contract;
        }

        private void Expand()
        {
            _expandForce1.relativeForce = Vector3.left * -_expandPower;
            _expandForce2.relativeForce = Vector3.left * _expandPower;
        }

        private void Contract()
        {

            _expandForce1.relativeForce = Vector3.left * _expandPower;
            _expandForce2.relativeForce = Vector3.left * - _expandPower;
        }
    }
}
