using UnityEngine;

using System;
using System.Collections.Generic;


namespace Botter.States
{
	public class GameStateManager
	{
		public string Name { get; private set; }
		public GameStateManager Parent { get; private set; }
		public IGameState ActiveState { get; private set; }


		public GameStateManager(string name, GameStateManager parent) 
		{
			Name = name;
			Parent = parent;
		}

		public void SetState(IGameState state)
		{
			if( state.Manager != this ) 
			{
				throw new InvalidOperationException("Foreign state");
			}

			if( ActiveState != null ) 
			{
				ActiveState.ExitState();
			}

			ActiveState = state;

			if( ActiveState != null )
			{
				ActiveState.EnterState();
			}
		}


	}
}