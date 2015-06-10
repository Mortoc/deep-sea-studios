using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Construction
{
    public class ConstructionUI : MonoBehaviour
    {
        private ConstructionState _state;
        private GameObject _currentToolUI = null;

        public void ShowToolUI(ConstructionTool tool)
        {
            if (_currentToolUI)
            {
                Destroy(_currentToolUI);
            }

            if(tool.UIPrefab)
            {
                _currentToolUI = Instantiate<GameObject>(tool.UIPrefab);
                _currentToolUI.transform.SetParent(transform);
            }
        }

        public void Init(ConstructionState state)
        {
            _state = state;
            ShowToolUI(_state.ActiveTool);
            GetComponentInChildren<ConstructionBackButton>().Init(state);
        }

        public void StructureSelected()
        {
            _state.TransitionToState<StructureTool>();
        }
    }
}
