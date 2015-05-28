using UnityEngine;

using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DSS.States.Test
{
    [TestFixture]
    internal class GameStateManager_Test
    {
        private class MockGameState : GameState
        {
            public int EnterCount { get; set; }
            public int ExitCount { get; set; }

            public override void EnterState()
            {
                base.EnterState();
                EnterCount++;
            }
            public override void ExitState()
            {
                base.ExitState();
                ExitCount++;
            }
        }

        private class MockGameState2 : GameState 
        {
            public int EnterCount { get; set; }
            public int ExitCount { get; set; }

            public override void EnterState()
            {
                base.EnterState();
                EnterCount++;
            }
            public override void ExitState()
            {
                base.ExitState();
                ExitCount++;
            }
        }

        private GameStateManager _gsm;

        [SetUp]
        public void Init()
        {
            var gsmObj = new GameObject("Test GameStateManager");
            _gsm = gsmObj.AddComponent<GameStateManager>();
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(_gsm.gameObject);
        }

        [Test]
        public void GameStateManagersCallEditAndEnterStates()
        {
            var stateA = _gsm.TransitionToState<MockGameState>();
            var stateB = _gsm.TransitionToState<MockGameState2>();

            Assert.AreEqual(stateA.EnterCount, 1);
            Assert.AreEqual(stateA.ExitCount, 1);
            Assert.AreEqual(stateB.EnterCount, 1);
            Assert.AreEqual(stateB.ExitCount, 0);
            Assert.AreEqual(_gsm.ActiveState, stateB);

            var secondStateA = _gsm.TransitionToState<MockGameState>();
            Assert.AreEqual(stateA.GetInstanceID(), secondStateA.GetInstanceID());
        }
    }
}
