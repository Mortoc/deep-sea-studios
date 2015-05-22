using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.UI
{
    public interface IHoverable
    {
        void OnHoverStart();
        void OnHoverEnd();
    }

    public interface IClickable
    {
        void OnClickAsButton();
    }

    public interface ISelectable
    {
        bool Selected { get; }
        void OnSelect();
        void OnDeselect();
    }
}
