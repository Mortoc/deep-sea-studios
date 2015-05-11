using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class Plug : MonoBehaviour, IHoverable
    {
        [SerializeField]
        private int _maxSiblings = 2;
        private List<Plug> _siblingPlugs = new List<Plug>();

        public event Action OnPower;
        public event Action OnPowerLoss;

        private Color _initialPlugRingEmissive;

        [SerializeField]
        private float _plugAnimateTime = 1.0f;
        private Coroutine _plugAnimation;

        private bool _powered = false;
        private bool _haveCheckedIfPowered = false;


        [SerializeField]
        private float _outlineSize = 0.25f;
        [SerializeField]
        private Material _outlineMaterial;
        private IEnumerable<GameObject> _outlines;

        void Awake()
        {
            _outlines = Outliner.BuildOutline(gameObject, _outlineSize, _outlineMaterial, true, 0.0f, true);
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

        private int _hoverFrame = -1;
        public void OnHover()
        {
            ShowOutlines(true);
            _hoverFrame = Time.frameCount;
        }

        private void ShowOutlines(bool on)
        {
            foreach(var outline in _outlines)
            {
                outline.SetActive(on);
            }
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


        private void RemoveSibling(Plug toRemove)
        {
            _siblingPlugs.RemoveAt(
                _siblingPlugs.FindIndex(x => x.GetInstanceID() == toRemove.GetInstanceID())
            );
        }

        private void AddSibling(Plug toAdd)
        {
            _siblingPlugs.Add(toAdd);
        }

        private bool CanAddSibling(Plug toAdd)
        {
            if (_siblingPlugs.Find(x => x.GetInstanceID() == toAdd.GetInstanceID()))
            {
                return false;
            }

            return _siblingPlugs.Count < _maxSiblings;
        }

        public static bool ConnectPlugs(Plug first, Plug second)
        {
            if (first.CanAddSibling(second) && second.CanAddSibling(first))
            {
                //Add Wire here?
                first.AddSibling(second);
                second.AddSibling(first);
                return true;
            }
            return false;
        } 

        private void UpdatePowerState()
        {
            bool wasPowered = _powered;
            _powered = IsPowered();
            _haveCheckedIfPowered = true;

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
        void LateUpdate()
        {
            _haveCheckedIfPowered = false;

            if (_hoverFrame != Time.frameCount)
            {
                ShowOutlines(false);
            }
        }

        public virtual bool IsPowered()
        {
            if (_haveCheckedIfPowered)
            {
                return _powered;
            }
            
            //foreach (var sibling in _siblingPlugs)
            //{
            //    if (sibling.IsPowered())
            //    {
            //        return true;
            //    }
            //}
            //return false;
            return true;
        }
    }
}
