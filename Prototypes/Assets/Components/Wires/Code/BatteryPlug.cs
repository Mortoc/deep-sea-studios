using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class BatteryPlug : Plug
    {
        public override bool IsPowered()
        {
            return true;
        }
    }
}
