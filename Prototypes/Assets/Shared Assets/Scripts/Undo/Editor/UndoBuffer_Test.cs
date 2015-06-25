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
        
        [Test]
        public void DoRunsTheCommandsDo()
        {
            var undoBuffer = new UndoBuffer();
            var doCount = 0;
            var command = new Command();
            command.DoFunc += () => doCount++;
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
            undoBuffer.Do(new Command());

            Assert.IsTrue(undoBuffer.CanUndo);
            undoBuffer.Undo();
            Assert.IsFalse(undoBuffer.CanUndo);
        }

        [Test]
        public void UndoCallsTheUndoFunctionOfTheCurrentCommand()
        {
            var undoBuffer = new UndoBuffer();
            var undoCount = 0;

            var command = new Command();
            command.UndoFunc += () => undoCount++;
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
            
            undoBuffer.Do(new Command());
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
            var command = new Command();
            command.DoFunc += () => doCount++;
            undoBuffer.Do(command);
            undoBuffer.Undo();
            undoBuffer.Redo();
            Assert.AreEqual(doCount, 2);
        }

        [Test]
        public void AddingANewCommandClearsTheRedoQueue()
        {
            var undoBuffer = new UndoBuffer();

            undoBuffer.Do(new Command());
            undoBuffer.Undo();

            Assert.IsTrue(undoBuffer.CanRedo);
            undoBuffer.Do(new Command());
            Assert.IsFalse(undoBuffer.CanRedo);
        }

        [Test]
        public void UndoQueueWorksMultipleLevelsDeep()
        {
            var undoBuffer = new UndoBuffer();
            undoBuffer.Do(new Command());
            undoBuffer.Do(new Command());
            undoBuffer.Do(new Command());
            undoBuffer.Do(new Command());
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
        
        [Test]
        public void CanUndoCallbacksFireOnlyWhenFirstAvailable()
        {
            var currentUndoState = false;
            var callbackCount = 0;
            var undoBuffer = new UndoBuffer();
            undoBuffer.OnCanUndo += s =>
            {
                currentUndoState = s;
                callbackCount++;
            };

            Assert.AreEqual(currentUndoState, undoBuffer.CanUndo);
            undoBuffer.Do(new Command());

            Assert.IsTrue(currentUndoState);
            Assert.AreEqual(currentUndoState, undoBuffer.CanUndo);
            Assert.AreEqual(1, callbackCount);

            undoBuffer.Do(new Command());
            Assert.AreEqual(1, callbackCount);
            
            undoBuffer.Undo();
            Assert.AreEqual(currentUndoState, undoBuffer.CanUndo);
            Assert.AreEqual(1, callbackCount);

            undoBuffer.Undo();
            Assert.AreEqual(2, callbackCount);
            Assert.IsFalse(currentUndoState);
            Assert.AreEqual(currentUndoState, undoBuffer.CanUndo);
        }

        [Test]
        public void CanRedoCallbacksFireOnlyWhenFirstAvailable()
        {
            var currentRedoState = false;
            var callbackCount = 0;
            var undoBuffer = new UndoBuffer();
            undoBuffer.OnCanRedo += s =>
            {
                currentRedoState = s;
                callbackCount++;
            };

            Assert.AreEqual(currentRedoState, undoBuffer.CanRedo);
            undoBuffer.Do(new Command());
            undoBuffer.Do(new Command());

            Assert.IsFalse(currentRedoState);
            Assert.AreEqual(currentRedoState, undoBuffer.CanRedo);

            undoBuffer.Undo();
            var callbackCountBefore2ndUndo = callbackCount;
            undoBuffer.Undo();
            Assert.AreEqual(callbackCountBefore2ndUndo, callbackCount);

            Assert.IsTrue(currentRedoState);
            Assert.AreEqual(currentRedoState, undoBuffer.CanRedo);
        }
    }
}