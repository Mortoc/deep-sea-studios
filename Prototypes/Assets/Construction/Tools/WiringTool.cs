using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class WiringTool : ConstructionTool
    {
        private Coroutine _toolExecutionLoop;
        private int _plugLayerMask;

        private void Start()
        {
            _plugLayerMask = 1 << LayerMask.NameToLayer("Plug");
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
            foreach (var rh in Physics.RaycastAll(ray, Mathf.Infinity, _plugLayerMask))
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

                if (Input.GetMouseButtonDown(0))
                {
                    var sceneRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var plug = GetPlugInRay(sceneRay);

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
