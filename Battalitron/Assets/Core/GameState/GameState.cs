using UnityEngine;

using System;
using System.Collections.Generic;


namespace BackstreetBots.States
{
	public abstract class GameState : MonoBehaviour
	{
        public event Action EnteredState;
        public event Action ExitedState;

		public GameStateManager Manager { get; private set; }
		
		public virtual void Init(GameStateManager manager) 
		{
            Manager = manager;
            transform.parent = manager.transform;
		}

        public virtual void EnterState()
        {
            if (EnteredState != null) EnteredState();
        }
        
        public virtual void ExitState()
        {
            if (ExitedState != null) ExitedState();
        }
	}
}