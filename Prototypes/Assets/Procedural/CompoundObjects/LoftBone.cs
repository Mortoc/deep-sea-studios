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

        public void OnDrawGizmos()
        {
            if (Last)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(transform.position, Last.transform.position);
            }
        }


        private IEnumerator KeepUpdated()
        {
            while(gameObject)
            {
                transform.position = Spline.PositionSample(T);
                transform.forward = Spline.ForwardSample(T);

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
