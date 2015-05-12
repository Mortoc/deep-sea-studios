using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public class WiringTool : ConstructionTool
    {
        private Coroutine _toolExecutionLoop;

        private void Start()
        {
            ToolMask = 1 << LayerMask.NameToLayer("Plug");
        }

        public override void OnSelect()
        {
            base.OnSelect();
            _toolExecutionLoop = StartCoroutine(DoWiring());
        }

        public override void OnDeselect()
        {
            base.OnDeselect();
            StopCoroutine(_toolExecutionLoop);
            if (_currentSelection)
            {
                _currentSelection.OnDeselect();
            }
        }

        private Plug GetPlugInRay(Ray ray)
        {
            foreach (var rh in Physics.RaycastAll(ray, Mathf.Infinity, ToolMask))
            {
                var plug = rh.collider.GetComponent<Plug>();
                if (plug)
                {
                    return plug;
                }
            }
            return null;
        }

        private Plug _currentSelection = null;
        private IEnumerator DoWiring()
        {
            while (gameObject)
            {
                yield return 0;
                var sceneRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                UpdateHoverStates(sceneRay);

                if (Input.GetMouseButtonDown(0))
                {
                    var plug = GetPlugInRay(sceneRay);
                    if( plug == _currentSelection )
                    {
                        continue;
                    }
                    if (plug && _currentSelection)
                    {
                        Plug.ConnectPlugs(plug, _currentSelection);
                    }
                    else if (plug)
                    {
                        _currentSelection = plug;
                        _currentSelection.OnSelect();
                    }
                    else
                    {
                        _currentSelection.OnDeselect();
                        _currentSelection = null;
                    }
                }
            }
        }
    }
}
