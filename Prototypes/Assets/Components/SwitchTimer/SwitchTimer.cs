using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class SwitchTimer : PowerableObject
    {
        [SerializeField]
        private float _toggleTime = 0.5f;

        [SerializeField]
        private GameObject _positiveIndicator;

        [SerializeField]
        private GameObject _positiveIndicatorLight;

        [SerializeField]
        private GameObject _negativeIndicator;

        [SerializeField]
        private GameObject _negativeIndicatorLight;


        [SerializeField]
        private Material _indicatorOffMaterial;

        [SerializeField]
        private Material _indicatorPositiveMaterial;

        [SerializeField]
        private Material _indicatorNegativeMaterial;

        void Awake()
        {
            base.Awake();
        }

        public override void On()
        {
            _positiveIndicator.GetComponent<Renderer>().material = _indicatorPositiveMaterial;
            _positiveIndicatorLight.SetActive(true);
            _negativeIndicator.GetComponent<Renderer>().material = _indicatorOffMaterial;
            _negativeIndicatorLight.SetActive(false);
        }
        public override void Off()
        {
            _positiveIndicator.GetComponent<Renderer>().material = _indicatorOffMaterial;
            _positiveIndicatorLight.SetActive(false);
            _negativeIndicator.GetComponent<Renderer>().material = _indicatorNegativeMaterial;
            _negativeIndicatorLight.SetActive(true);
        }
    }
}
