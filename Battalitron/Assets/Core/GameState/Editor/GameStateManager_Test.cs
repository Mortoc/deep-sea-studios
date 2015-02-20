using UnityEngine;

using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace BackstreetBots.States.Test
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
                EnterCount++;
            }
            public override void ExitState()
            {
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisteringAForeignTestThrows()
        {
            var subGsm = _gsm.CreateSubManager<GameStateManager>("Sub");

            var state = subGsm.CreateSubState<MockGameState>();

            _gsm.SetState(state);
        }

        [Test]
        public void GameStateManagersCallEditAndEnterStates()
        {
            var stateA = _gsm.CreateSubState<MockGameState>();
            var stateB = _gsm.CreateSubState<MockGameState>();

            _gsm.SetState(stateA);

            Assert.AreEqual(stateA.EnterCount, 1);
            Assert.AreEqual(stateA.ExitCount, 0);
            Assert.AreEqual(_gsm.ActiveState, stateA);

            _gsm.SetState(stateB);
            Assert.AreEqual(stateA.EnterCount, 1);
            Assert.AreEqual(stateA.ExitCount, 1);

            Assert.AreEqual(stateB.EnterCount, 1);
            Assert.AreEqual(stateB.ExitCount, 0);
            Assert.AreEqual(_gsm.ActiveState, stateB);

            _gsm.SetState(stateA);
            Assert.AreEqual(stateA.EnterCount, 2);
            Assert.AreEqual(stateA.ExitCount, 1);

            Assert.AreEqual(stateB.EnterCount, 1);
            Assert.AreEqual(stateB.ExitCount, 1);
            Assert.AreEqual(_gsm.ActiveState, stateA);
        }
    }
}
