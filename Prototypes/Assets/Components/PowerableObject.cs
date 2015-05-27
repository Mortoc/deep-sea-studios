using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;
namespace DSS
{
    public abstract class PowerableObject : MonoBehaviour
    {
        public event Action OnPower;
        public event Action OnPowerLoss;

        public List<Plug> _plugs = new List<Plug>();

        virtual public void TurnPowerOn()
        {
            _powered = true;
            var wires = GetComponents<Wire>();
            foreach (var wire in wires)
            {
                wire.PowerOn();
            }
            PowerOthers();
            OnPower();
        }

        virtual public void TurnPowerOff()
        {
            _powered = false;
            OnPowerLoss();
        }

        virtual public void PowerOthers()
        {
            _haveCheckedIfPowered = true;
            foreach (var plug in _plugs)
            {
                foreach (var sibling in plug._siblingPlugs)
                {
                    if (!sibling.powerableParent._haveCheckedIfPowered)
                    {
                        sibling.powerableParent.TurnPowerOn();
                    }
                }
            }
        }

        virtual public bool IsPowered()
        {
            return _powered;
        }

        public virtual void OnEnable()
        {
            OnPower += On;
            OnPowerLoss += Off;
        }

        public virtual void OnDisable()
        {
            OnPower -= On;
            OnPowerLoss -= Off;
        }

        //The actions the object does when on
        abstract public void On();
        abstract public void Off();

        public virtual void Awake()
        {
            if (_plugs.Count == 0)
            {
                Debug.LogError(this.name + " missing plug");
            }
            foreach (var p in _plugs)
            {
                p.powerableParent = this;
            } 
        }

        virtual public void Update()
        {
            _haveCheckedIfPowered = false;
            _powered = false;
        }

        public bool _haveCheckedIfPowered
        {
            get; set;
        }
        public bool _powered
        {
            get; set;
        }
    }
}