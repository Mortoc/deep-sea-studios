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
        private int _maxWires = 2;
        public List<Wire> _connectedWires = new List<Wire>();

        public event Action OnPower;
        public event Action OnPowerLoss;
        
        private bool _powered = false;
        private bool _wasPowered = false;
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
        
        private void RemoveWire(Wire toRemove)
        {
            _connectedWires.Remove(toRemove);
        }

        private void AddWire(Wire toAdd)
        {
            _connectedWires.Add(toAdd);
        }

        private bool CanConnectPlug(Plug toAdd)
        {
            if (_connectedWires.Count >= _maxWires)
                return false;

            foreach (var w in _connectedWires)
            {
                if (w.In == toAdd || w.Out == toAdd)
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

                first.AddWire(wire);
                second.AddWire(wire);

                return wire;
            }
            return null;
        }

        void Update()
        {
            _wasPowered = _powered;
            _powered = false;
            _haveCheckedIfPowered = false;
        }

        public void PowerOn()
        {
            if (_haveCheckedIfPowered)
                return;
            _haveCheckedIfPowered = true;
            _powered = true;
            
            foreach(var w in _connectedWires)
            {
                w.PowerOn();
                w.In.PowerOn();
                w.Out.PowerOn();
            }
        }

        public virtual bool IsPowered()
        {
            return _powered;
        }
    }
}
