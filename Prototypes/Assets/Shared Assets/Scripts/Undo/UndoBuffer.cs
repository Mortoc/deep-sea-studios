using UnityEngine;

using System;
using System.Collections.Generic;

namespace DSS
{
    public interface ICommand
    {
        void Do();
        void Undo();
    }

    public class UndoBuffer
    {
        private List<ICommand> _undoHistory = new List<ICommand>();

        // should always be an index in _undoHistory or -1 when _undoHistory is empty
        private int _currentCommand = -1; 

        private void DiscardRedoBuffer()
        {
            if (_currentCommand < _undoHistory.Count - 1)
            {
                _undoHistory.RemoveRange(_currentCommand + 1, (_undoHistory.Count - 1) - _currentCommand);
            }
        }

        public void Do(ICommand command)
        {
            DiscardRedoBuffer();

            command.Do();
            _undoHistory.Add(command);
            _currentCommand++;
        }

        public bool CanUndo
        {
            get { return _currentCommand >= 0; }
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                throw new InvalidOperationException("Nothing to undo");
            }
            _undoHistory[_currentCommand].Undo();
            _currentCommand--;
        }
        
        public bool CanRedo
        {
            get { return _currentCommand < _undoHistory.Count - 1; }
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                throw new InvalidOperationException("Nothing to redo");
            }
            _currentCommand++;
            _undoHistory[_currentCommand].Do();
        }
    }

}
