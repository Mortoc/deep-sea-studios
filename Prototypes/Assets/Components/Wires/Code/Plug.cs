using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.UI;

namespace DSS
{
    public class Plug : MonoBehaviour, IHoverable, ISelectable
    {
        [SerializeField]
        private int _maxSiblings = 2;
        public List<Plug> _siblingPlugs = new List<Plug>();


        public PowerableObject powerableParent;

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
        }

        void OnDisable()
        {
        }

        public void OnHoverStart()
        {
            if( !Selected )
            {
                GetComponent<Renderer>().materials = _hoveredMaterials;
            }
        }

        public void OnHoverEnd()
        {
            if (!Selected)
            {
                GetComponent<Renderer>().materials = _originalMaterials;
            }
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
        
        private void RemoveWire(Plug toRemove)
        {
            _siblingPlugs.Remove(toRemove);
        }

        private void AddWire(Plug toAdd)
        {
            _siblingPlugs.Add(toAdd);
        }

        private bool CanConnectPlug(Plug toAdd)
        {
            if (_siblingPlugs.Count >= _maxSiblings)
                return false;

            foreach (var w in _siblingPlugs)
            {
                if (w == toAdd || w == toAdd)
                {
                    return false;
                }
            }
            
            return true;
        }

        public static Wire ConnectPlugs(Plug first, Plug second, Material wireOnMaterial, Material wireOffMaterial)
        {
            if (first.CanConnectPlug(second) && second.CanConnectPlug(first))
            {
                var wireObj = new GameObject("Wire");
                var wire = wireObj.AddComponent<Wire>();
                wire.SetupWire(first, second, wireOnMaterial, wireOffMaterial);

                first.AddWire(second);
                second.AddWire(first);

                return wire;
            }
            return null;
        }

        void Update()
        {
        }
    }
}
