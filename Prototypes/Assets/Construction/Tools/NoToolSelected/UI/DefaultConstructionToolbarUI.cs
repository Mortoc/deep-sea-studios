using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public class DefaultConstructionToolbarUI : MonoBehaviour
    {
        public void StructureSelected()
        {
            FindObjectOfType<ConstructionUI>().StructureSelected();
        }
    }
}
