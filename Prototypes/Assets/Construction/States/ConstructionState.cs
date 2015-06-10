using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Construction
{
    public class ConstructionState : GameState
    {
        [SerializeField]
        private GameObject _constructionScreenGUIPrefab;
        private ConstructionUI _constructionScreenGUI;

        public ConstructionTool ActiveTool
        {
            get
            {
                return this.ActiveState as ConstructionTool;
            } 
        }

        public override void EnterState()
        {
            base.EnterState();

            TransitionToState<NoToolSelected>();

            _constructionScreenGUI = Instantiate(_constructionScreenGUIPrefab).GetComponent<ConstructionUI>();
            _constructionScreenGUI.Init(this);
        }

        public override void ExitState()
        {
            base.ExitState();
            Destroy(_constructionScreenGUI.gameObject);
        }

    }
}
