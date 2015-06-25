using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public class DefaultConstructionToolbarUI : ConstructionToolbarUI
    {
        public override void Init(ConstructionTool tool) {}

        public void StructureSelected()
        {
            FindObjectOfType<ConstructionUI>().StructureSelected();
        }
    }
}
