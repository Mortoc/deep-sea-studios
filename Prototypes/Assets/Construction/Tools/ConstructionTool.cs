using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public abstract class ConstructionTool : MonoBehaviour, ISelectable
    {
        public bool Selected { get; private set; }
        public virtual void OnSelect()
        {
            Selected = true;
        }

        public virtual void OnDeselect()
        {
            Selected = false;
        }

        public int ToolMask { get; protected set; }
        
        public void UpdateHoverStates(Ray? reuseCameraRay = null)
        {
            if (ToolMask == 0)
            {
                throw new InvalidOperationException("ToolMask was never set up");
            }

            Ray cameraRay;
            if( reuseCameraRay != null )
            {
                cameraRay = (Ray)reuseCameraRay;
            }
            else
            {
                cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            }

            var objectsUnderCursor = Physics.RaycastAll(cameraRay, Mathf.Infinity, ToolMask);
            objectsUnderCursor.OrderBy(rh => rh.collider.gameObject.GetInstanceID());

            var lastInstanceId = -1;
            foreach(var objectUnderCursor in objectsUnderCursor)
            {
                var instanceId = objectUnderCursor.collider.gameObject.GetInstanceID(); 
                if( lastInstanceId != instanceId )
                {
                    lastInstanceId = instanceId;
                    var monoBehaviors = objectUnderCursor.collider.gameObject.GetComponentsInChildren<MonoBehaviour>();
                    foreach(IHoverable hoverable in monoBehaviors.Where(mb => mb is IHoverable).Distinct())
                    {
                        hoverable.OnHover();
                    }
                }
            }
            
        }
        
    }
}
