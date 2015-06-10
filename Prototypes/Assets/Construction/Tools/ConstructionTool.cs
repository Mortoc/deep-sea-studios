using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Construction
{
    public abstract class ConstructionTool : GameState
    {
        public GameObject UIPrefab;

        public int ToolMask { get; protected set; }

        public override void EnterState()
        {
            base.EnterState();
            FindObjectOfType<ConstructionUI>().ShowToolUI(this);
        }

        public virtual void BackButtonAction()
        {
            Parent.TransitionToState<NoToolSelected>();
        }
    }
}
