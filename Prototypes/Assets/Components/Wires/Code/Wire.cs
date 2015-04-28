using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class Wire : MonoBehaviour
    {
    
        [SerializeField]
        private Plug _in;
        public Plug In
        {
            get { return _in; }
        }

        [SerializeField]
        private Plug _out;
        public Plug Out
        {
            get { return _out; }
        }

        private static Color _gizmoColor = new Color(1.0f, 0.5f, 0.4f, 1.0f);

        public void OnDrawGizmos()
        {
            if (_in && _out)
            {
                Gizmos.color = _gizmoColor;
                Gizmos.DrawLine(_in.transform.position, _out.transform.position);
            }
        }
    }
}
