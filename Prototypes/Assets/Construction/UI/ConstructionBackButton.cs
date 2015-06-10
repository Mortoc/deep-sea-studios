using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public class ConstructionBackButton : MonoBehaviour
    {
        private ConstructionState _state;

        public void Init(ConstructionState state)
        {
            _state = state;
        }

        public void Back()
        {
            _state.ActiveTool.BackButtonAction();
        }
    }
}
