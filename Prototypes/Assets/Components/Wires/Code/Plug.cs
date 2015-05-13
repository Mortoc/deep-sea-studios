using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class Plug : MonoBehaviour, IHoverable, ISelectable
    {
        [SerializeField]
        private int _maxSiblings = 2;
        private List<Plug> _siblingPlugs = new List<Plug>();

        public event Action OnPower;
        public event Action OnPowerLoss;
        
        private bool _powered = false;
        private bool _haveCheckedIfPowered = false;


        [SerializeField]
        private Material _hoverMaterial;
        private Material[] _hoveredMaterials;

        [SerializeField]
        private Material _selectMaterial;
        private Material[] _selectedMaterials;

        private Material[] _originalMaterials;

        void Awake()
        {
            _originalMaterials = GetComponent<Renderer>().materials;

            _hoveredMaterials = (Material[])_originalMaterials.Clone();
            _hoveredMaterials[1] = _hoverMaterial;

            _selectedMaterials = (Material[])_originalMaterials.Clone();
            _selectedMaterials[1] = _selectMaterial;
        }

        void OnEnable()
        {
            UpdatePowerState();
        }

        void OnDisable()
        {
        }

        private int _hoverFrame = -1;
        private bool _hovered = false;
        public void OnHover()
        {
            if( !_hovered && !Selected )
            {
                GetComponent<Renderer>().materials = _hoveredMaterials;
            }

            _hoverFrame = Time.frameCount;
            _hovered = true;
        }

        public bool Selected { get; private set; }
        public void OnSelect()
        {
            Selected = true;
            GetComponent<Renderer>().materials = _selectedMaterials;
        }

        public void OnDeselect()
        {
            Selected = false;
            GetComponent<Renderer>().materials = _originalMaterials;
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

        public static Wire ConnectPlugs(Plug first, Plug second, Material wireOnMaterial, Material wireOffMaterial)
        {
            if (first.CanAddSibling(second) && second.CanAddSibling(first))
            {
                first.AddSibling(second);
                second.AddSibling(first);
                var wireObj = new GameObject("Wire");
                var wire = wireObj.AddComponent<Wire>();
                wire.SetupWire(first, second, wireOnMaterial, wireOffMaterial);
                return wire;
            }
            return null;
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

            if (_hovered && _hoverFrame != Time.frameCount)
            {
                _hovered = false;

                if( !Selected )
                {
                    GetComponent<Renderer>().materials = _originalMaterials;
                }
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
