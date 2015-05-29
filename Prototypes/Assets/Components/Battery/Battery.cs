using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.Procedural;

namespace DSS
{
    public class Battery : PowerableObject
    {
        public override bool IsPowered()
        {
            return true;
        }

        public override void Off(){}
        public override void On(){}
        public override void TurnPowerOff(){}
        public override void TurnPowerOn(){}

        public override void Awake()
        {
            base.Awake();
        }

        void LateUpdate()
        {
            PowerOthers();
        }
    }
}