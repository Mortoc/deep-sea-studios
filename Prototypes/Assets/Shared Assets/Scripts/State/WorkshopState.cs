using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.Construction;

namespace DSS.States
{
    public class WorkshopState : GameState
    {
        // Stuff happening on start because this is the root state object
        public void Start()
        {
            this.Init(null);
            GoToHomeState();
            
        }

        public void GoToHomeState()
        {
            SetState(GetComponentsInChildren<HomeState>(true)[0]);
        }

        public void GoToConstructionState()
        {
            SetState(GetComponentsInChildren<ConstructionState>(true)[0]);
        }
    }
}
