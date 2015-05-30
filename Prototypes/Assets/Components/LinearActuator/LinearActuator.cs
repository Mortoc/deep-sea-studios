using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class LinearActuator : PowerableObject
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

        public override void Awake()
        {
            base.Awake();
            IgnoreAllCollisions(GetComponentsInChildren<Collider>());
        }

        public override void On()
        {
            if (Rand.value > 0.5f)
            {
                _expandForce1.relativeForce = Vector3.left * -_expandPower;
                _expandForce2.relativeForce = Vector3.left * _expandPower;
            }
            else
            {
                _expandForce1.relativeForce = Vector3.left * _expandPower;
                _expandForce2.relativeForce = Vector3.left * -_expandPower;
            }
        }

        public override void Off()
        {
            _expandForce1.relativeForce = Vector3.left * _expandPower;
            _expandForce2.relativeForce = Vector3.left * - _expandPower;
        }
    }
}
