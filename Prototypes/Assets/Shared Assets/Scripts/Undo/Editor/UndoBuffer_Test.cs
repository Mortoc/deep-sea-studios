using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using UnityEngine;
using UnityTest;

namespace DSS.UnitTests
{
    [TestFixture]
    [Category("Tool Tests")]
    internal class UndoBuffer_Test
    {
        private class MockCommand : ICommand
        {
            public event Action DoImpl;
            public event Action UndoImpl;

            public void Do()
            {
                if (DoImpl != null)
                {
                    DoImpl();
                }
            }

            public void Undo()
            {
                if (UndoImpl != null)
                {
                    UndoImpl();
                }
            }
        }
        
        [Test]
        public void DoRunsTheCommandsDo()
        {

            var undoBuffer = new UndoBuffer();
            var doCount = 0;
            var command = new MockCommand();
            command.DoImpl += () => doCount++;
            undoBuffer.Do(command);

            Assert.AreEqual(doCount, 1);
        }

        [Test]
        public void UndoIsNotAvailableBeforeAnyCommandsAreSent()
        {
            var undoBuffer = new UndoBuffer();
            Assert.Throws<InvalidOperationException>(() =>
            {
                undoBuffer.Undo();
            });

            Assert.IsFalse(undoBuffer.CanUndo);
        }
        
        [Test]
        public void UndoIsAvailableAfterCommandsAreSent()
        {
            var undoBuffer = new UndoBuffer();
            undoBuffer.Do(new MockCommand());

            Assert.IsTrue(undoBuffer.CanUndo);
            undoBuffer.Undo();
            Assert.IsFalse(undoBuffer.CanUndo);
        }

        [Test]
        public void UndoCallsTheUndoFunctionOfTheCurrentCommand()
        {
            var undoBuffer = new UndoBuffer();
            var undoCount = 0;

            var command = new MockCommand();
            command.UndoImpl += () => undoCount++;
            undoBuffer.Do(command);

            Assert.IsTrue(undoBuffer.CanUndo);
            undoBuffer.Undo();
            Assert.IsFalse(undoBuffer.CanUndo);

            Assert.AreEqual(undoCount, 1);
        }

        [Test]
        public void RedoIsNotAvailableUntilAnUndoHappened()
        {
            var undoBuffer = new UndoBuffer();
            Assert.Throws<InvalidOperationException>(() =>
            {
                undoBuffer.Redo();
            });

            Assert.IsFalse(undoBuffer.CanRedo);
        }

        [Test]
        public void RedoIsAvailableAfterAnUndoHappened()
        {
            var undoBuffer = new UndoBuffer();
            
            undoBuffer.Do(new MockCommand());
            undoBuffer.Undo();
            
            Assert.IsTrue(undoBuffer.CanRedo);
            undoBuffer.Redo();
            Assert.IsFalse(undoBuffer.CanRedo);
        }
        
        [Test]
        public void RedoCallsTheDoCommand()
        {
            var undoBuffer = new UndoBuffer();
            var doCount = 0;
            var command = new MockCommand();
            command.DoImpl += () => doCount++;
            undoBuffer.Do(command);
            undoBuffer.Undo();
            undoBuffer.Redo();
            Assert.AreEqual(doCount, 2);
        }

        [Test]
        public void AddingANewCommandClearsTheRedoQueue()
        {
            var undoBuffer = new UndoBuffer();

            undoBuffer.Do(new MockCommand());
            undoBuffer.Undo();

            Assert.IsTrue(undoBuffer.CanRedo);
            undoBuffer.Do(new MockCommand());
            Assert.IsFalse(undoBuffer.CanRedo);
        }

        [Test]
        public void UndoQueueWorksMultipleLevelsDeep()
        {
            var undoBuffer = new UndoBuffer();
            undoBuffer.Do(new MockCommand());
            undoBuffer.Do(new MockCommand());
            undoBuffer.Do(new MockCommand());
            undoBuffer.Do(new MockCommand());
            Assert.IsTrue(undoBuffer.CanUndo);
            Assert.IsFalse(undoBuffer.CanRedo);

            undoBuffer.Undo();
            Assert.IsTrue(undoBuffer.CanUndo);
            Assert.IsTrue(undoBuffer.CanRedo);

            undoBuffer.Undo();
            Assert.IsTrue(undoBuffer.CanUndo);
            Assert.IsTrue(undoBuffer.CanRedo);

            undoBuffer.Undo();
            Assert.IsTrue(undoBuffer.CanUndo);
            Assert.IsTrue(undoBuffer.CanRedo);

            undoBuffer.Undo();
            Assert.IsFalse(undoBuffer.CanUndo);
            Assert.IsTrue(undoBuffer.CanRedo);

            undoBuffer.Redo();
            Assert.IsTrue(undoBuffer.CanUndo);
            Assert.IsTrue(undoBuffer.CanRedo);

            undoBuffer.Redo();
            Assert.IsTrue(undoBuffer.CanUndo);
            Assert.IsTrue(undoBuffer.CanRedo);

            undoBuffer.Redo();
            Assert.IsTrue(undoBuffer.CanUndo);
            Assert.IsTrue(undoBuffer.CanRedo);

            undoBuffer.Redo();
            Assert.IsTrue(undoBuffer.CanUndo);
            Assert.IsFalse(undoBuffer.CanRedo);
        }
    }
}