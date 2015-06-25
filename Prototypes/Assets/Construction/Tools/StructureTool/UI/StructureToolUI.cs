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
        [SerializeField]
        private Button _undoButton;

        [SerializeField]
        private Button _redoButton;

        private StructureTool _tool;

        public override void Init(ConstructionTool tool)
        {
            _tool = (StructureTool)tool;

            _tool.CommandBuffer.OnCanUndo += EnableDisableUndoButton;
            _tool.CommandBuffer.OnCanRedo += EnableDisableRedoButton;
            EnableDisableUndoButton(false);
            EnableDisableRedoButton(false);
        }

        void OnDisable()
        {
            _tool.CommandBuffer.OnCanUndo -= EnableDisableUndoButton;
            _tool.CommandBuffer.OnCanRedo -= EnableDisableRedoButton;
        }

        private void EnableDisableUndoButton(bool enabled)
        {
            _undoButton.interactable = enabled;
        }

        private void EnableDisableRedoButton(bool enabled)
        {
            _redoButton.interactable = enabled;
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