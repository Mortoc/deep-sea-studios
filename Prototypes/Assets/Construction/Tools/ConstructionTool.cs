using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Rand = UnityEngine.Random;

using DSS.UI;
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

        private int _maxHovers;
        protected int MaxHovers
        {
            get { return _maxHovers; }
            set
            {
                _maxHovers = value;
                foreach (var ho in _hoveredLastFrame)
                {
                    if (ho != null)
                    {
                        ho.OnHoverEnd();
                    }
                }
                _hoveredLastFrame = new IHoverable[_maxHovers];
            }
        }
        private IHoverable[] _hoveredLastFrame = new IHoverable[4];

        public void UpdateHoverStates(Ray? reuseCameraRay = null)
        {
            if (ToolMask == 0)
            {
                throw new InvalidOperationException("ToolMask was never set up");
            }

            Ray cameraRay;
            if (reuseCameraRay != null)
            {
                cameraRay = (Ray)reuseCameraRay;
            }
            else
            {
                cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            }

            var raycastHits = Physics.RaycastAll(cameraRay, Mathf.Infinity, ToolMask);

            var hoveredObjects = new List<IHoverable>();
            foreach (var rh in raycastHits)
            {
                foreach (var mb in rh.collider.GetComponentsInChildren<MonoBehaviour>())
                {
                    if (mb is IHoverable && hoveredObjects.Count() < MaxHovers)
                    {
                        hoveredObjects.Add(mb as IHoverable);
                    }
                }
            }

            // Newly hovered objects
            foreach (var ho in hoveredObjects.Where(ho => !_hoveredLastFrame.Contains(ho)))
            {
                ho.OnHoverStart();
            }

            // No longer hovered objects
            foreach (var ho in _hoveredLastFrame.Where(ho => !hoveredObjects.Contains(ho) && ho != null))
            {
                ho.OnHoverEnd();
            }

            for (int i = 0; i < MaxHovers; ++i)
            {
                if (i < hoveredObjects.Count())
                {
                    _hoveredLastFrame[i] = hoveredObjects.ElementAt(i);
                }
                else
                {
                    _hoveredLastFrame[i] = null;
                }
            }
        }

    }
}
