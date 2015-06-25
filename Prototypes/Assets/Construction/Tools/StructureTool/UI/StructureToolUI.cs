using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Construction
{
    public class StructureToolUI : ConstructionToolbarUI
    {
        private StructureTool _tool;

        public override void Init(ConstructionTool tool)
        {
            _tool = (StructureTool)tool;
        }

        public void Undo()
        {
            if (_tool.CommandBuffer.CanUndo)
            {
                _tool.CommandBuffer.Undo();
            }
        }

        public void Redo()
        {
            if (_tool.CommandBuffer.CanRedo)
            {
                _tool.CommandBuffer.Redo();
            }
        }
    }
}