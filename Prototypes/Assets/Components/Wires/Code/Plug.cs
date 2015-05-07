using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class Plug : MonoBehaviour
    {
        [SerializeField]
        private Wire _connectedWire;

        public event Action OnPower;
        public event Action OnPowerLoss;

        private Color _initialPlugRingEmissive;

        [SerializeField]
        private float _plugAnimateTime = 1.0f;
        private Coroutine _plugAnimation;

        private bool _powered = false;

        void Awake()
        {
            _initialPlugRingEmissive = GetComponent<Renderer>().materials[1].GetColor("_EmissionColorUI");
        }

        void OnEnable()
        {
            OnPower += TurnOnPlug;
            OnPowerLoss += TurnOffPlug;
            UpdatePowerState();
        }

        void OnDisable()
        {
            OnPower -= TurnOnPlug;
            OnPowerLoss -= TurnOffPlug;
        }

        private void TurnOnPlug()
        {
            if (_plugAnimation != null)
            {
                StopCoroutine(_plugAnimation);
            }

            _plugAnimation = StartCoroutine(AnimatePlugEmissive(_initialPlugRingEmissive));
        }

        private void TurnOffPlug()
        {
            if (_plugAnimation != null)
            {
                StopCoroutine(_plugAnimation);
            }

            _plugAnimation = StartCoroutine(AnimatePlugEmissive(Color.black));
        }

        private IEnumerator AnimatePlugEmissive(Color toEmissive)
        {
            var invTime = 1.0f / _plugAnimateTime;
            var mat = GetComponent<Renderer>().materials[1];
            var initialEmissive = mat.GetColor("_EmissionColor");
            
            for (var t = 0.0f; t < _plugAnimateTime; t += Time.deltaTime)
            {
                var smoothT = Mathf.SmoothStep(0.0f, 1.0f, invTime * t);
                var emissive = Color.Lerp(initialEmissive, toEmissive, smoothT);
                mat.SetColor("_EmissionColor", emissive);
                yield return 0;
            }
            mat.SetColor("_EmissionColorUI", toEmissive);
        }

        private void UpdatePowerState()
        {
            bool wasPowered = _powered;
            _powered = IsPoweredImpl();

            if( wasPowered != _powered && _powered && OnPower != null )
            {
                OnPower();
            }
            else if( wasPowered != _powered && !_powered && OnPowerLoss != null )
            {
                OnPowerLoss();
            }
        }

        void Update()
        {
            UpdatePowerState();
        }

        private bool IsPoweredImpl()
        {
            if (_connectedWire && _connectedWire.In)
            {
                return _connectedWire.In.IsPowered();
            }
            else
            {
                return false;
            }
        }

        public virtual bool IsPowered()
        {
            return _powered;
        }
    }
}
