using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public interface ISelectable
    {
        bool Selected { get; }
        void OnSelect();
        void OnDeselect();
    }
}
