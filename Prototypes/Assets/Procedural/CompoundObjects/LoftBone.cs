using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Procedural
{
    public class LoftBone : MonoBehaviour
    {
        public ISpline Spline { get; private set; }
        public float T { get; set; }
        public LoftBone Last { get; private set; }
        public bool OnFixedUpdate { get; set; }

        public void Init(ISpline spline, float t, LoftBone last = null)
        {
            Spline = spline;
            T = t;
            Last = last;

            StartCoroutine(KeepUpdated());
        }

        private IEnumerator KeepUpdated()
        {
            while(gameObject)
            {
                transform.position = transform.parent.TransformPoint(Spline.PositionSample(T));
                transform.forward = transform.parent.TransformVector(Spline.ForwardSample(T));

                if( OnFixedUpdate )
                {
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    yield return 0;
                }
            }
        }
    }
}
