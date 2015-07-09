using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Construction
{
	public class NoToolSelected : ConstructionTool
	{
        public override void BackButtonAction()
        {
            FindObjectOfType<WorkshopState>().TransitionToState<HomeState>();
        }
    }
}
