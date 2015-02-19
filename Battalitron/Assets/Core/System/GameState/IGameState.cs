using UnityEngine;

using System;
using System.Collections.Generic;


namespace Botter.States
{
	public interface IGameState
	{
		GameStateManager Manager { get; }

		void EnterState();
		void ExitState();
	}

	public abstract class BasicState : IGameState
	{
		public GameStateManager Manager { get; private set; }
		
		public BasicState(GameStateManager manager) 
		{
			Manager = manager;
		}
		
		public abstract void EnterState();
		public abstract void ExitState();
	}
}