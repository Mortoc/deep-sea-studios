using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class SwitchTimer : MonoBehaviour
    {
        [SerializeField]
        private float _toggleTime = 0.5f;

        [SerializeField]
        private GameObject _positiveIndicator;

        [SerializeField]
        private GameObject _negativeIndicator;
        

        [SerializeField]
        private Material _indicatorOffMaterial;

        [SerializeField]
        private Material _indicatorPositiveMaterial;

        [SerializeField]
        private Material _indicatorNegativeMaterial;

        [SerializeField]
        private Plug _plug1;

        [SerializeField]
        private Plug _plug2;

        private bool _isPowering = false;

        void Update()
        {
            _isPowering = Mathf.Sin(Time.time / _toggleTime) > 0.0f; 
            if( _isPowering )
            {
                _positiveIndicator.GetComponent<Renderer>().material = _indicatorPositiveMaterial;
                
                _negativeIndicator.GetComponent<Renderer>().material = _indicatorOffMaterial;
                
            }
            else
            {
                _positiveIndicator.GetComponent<Renderer>().material = _indicatorOffMaterial;
                
                _negativeIndicator.GetComponent<Renderer>().material = _indicatorNegativeMaterial;
                
            }
        }
    }
}
