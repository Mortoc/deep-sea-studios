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

        public override void Selected()
        {
            _toolExecutionLoop = StartCoroutine(DoWiring());
        }

        public override void Unselected()
        {
            StopCoroutine(_toolExecutionLoop);
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


        private IEnumerator DoWiring()
        {
            Plug currentSelection = null;
            while (gameObject)
            {
                yield return 0;
                var sceneRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                UpdateHoverStates(sceneRay);

                if (Input.GetMouseButtonDown(0))
                {
                    var plug = GetPlugInRay(sceneRay);
                    if( plug == currentSelection )
                    {
                        continue;
                    }
                    if (plug && currentSelection)
                    {
                        Plug.ConnectPlugs(plug, currentSelection);
                    }
                    else if (plug)
                    {
                        currentSelection = plug;
                    }
                    else
                    {
                        currentSelection = null;
                    }
                }
            }
        }
    }
}
