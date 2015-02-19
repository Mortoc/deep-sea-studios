using UnityEngine;

using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Botter.States.Test
{
	[TestFixture]
	internal class GameStateManager_Test 
	{
		private class MockGameState : BasicState
		{
			public int EnterCount { get; set; }
			public int ExitCount { get; set; }

			public MockGameState(GameStateManager manager)
				: base(manager)
			{
			}

			public override void EnterState ()
			{
				EnterCount++;
			}
			public override void ExitState ()
			{
				ExitCount++;
			}
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void RegisteringAForeignTestThrows()
		{
			var gsmA = new GameStateManager("A", null);
			var gsmB = new GameStateManager("B", null);

			var stateB = new MockGameState(gsmB);

			gsmA.SetState(stateB);
		}

		[Test]
		public void GameStateManagersCallEditAndEnterStates()
		{
			var gsm = new GameStateManager("TestManager", null);
			var stateA = new MockGameState(gsm);
			var stateB = new MockGameState(gsm);

			gsm.SetState(stateA);

			Assert.That(stateA.EnterCount, Is.EqualTo(1));
			Assert.That(stateA.ExitCount, Is.EqualTo(0));
			Assert.That(gsm.ActiveState, Is.EqualTo(stateA));
			
			gsm.SetState(stateB);
			Assert.That(stateA.EnterCount, Is.EqualTo(1));
			Assert.That(stateA.ExitCount, Is.EqualTo(1));
			
			Assert.That(stateB.EnterCount, Is.EqualTo(1));
			Assert.That(stateB.ExitCount, Is.EqualTo(0));
			Assert.That(gsm.ActiveState, Is.EqualTo(stateB));

			gsm.SetState(stateA);
			Assert.That(stateA.EnterCount, Is.EqualTo(2));
			Assert.That(stateA.ExitCount, Is.EqualTo(1));
			
			Assert.That(stateB.EnterCount, Is.EqualTo(1));
			Assert.That(stateB.ExitCount, Is.EqualTo(1));
			Assert.That(gsm.ActiveState, Is.EqualTo(stateA));
		}
	}
}
