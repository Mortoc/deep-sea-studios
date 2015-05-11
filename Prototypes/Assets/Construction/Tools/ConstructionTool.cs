using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public abstract class ConstructionTool : MonoBehaviour
    {
        public abstract void Selected();
        public abstract void Unselected();
    }
}
