using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Construction
{
    public class ConstructionUI : GameStateManager
    {
        void Awake()
        {
            foreach(var tool in GetComponentsInChildren<ConstructionTool>())
            {
                tool.Init(this);
            }
        }

        public void SelectTool(ConstructionTool tool)
        {
            SetState(tool);
        }   
    }
}
