﻿using UnityEngine;

using System;
using System.Collections.Generic;


namespace BackstreetBots.States
{
	public class GameStateManager : MonoBehaviour
	{
		public string Name { get; private set; }
		public GameStateManager Parent { get; private set; }

        [SerializeField]
		public GameState ActiveState;

        public event Action<GameState> NewState;

		public virtual void Init(string name, GameStateManager parent)
		{
			Name = name;
			Parent = parent;
            if (parent != null)
            {
                transform.parent = parent.transform;
            }
		}

		private void SetState(GameState state)
		{
			if( state.Manager != this ) 
			{
				throw new InvalidOperationException("Foreign state");
			}

			if( ActiveState != null ) 
			{
				ActiveState.ExitState();
                ActiveState.gameObject.SetActive(false);
			}

			ActiveState = state;

			if( ActiveState != null )
            {
                ActiveState.gameObject.SetActive(true);
				ActiveState.EnterState();
			}

            if (NewState != null) NewState(ActiveState);
		}

        public T CreateSubManager<T>(string name) where T : GameStateManager
        {
            var managerObj = new GameObject(name);
            var manager = managerObj.AddComponent<T>();
            manager.Init(name, this);
            return manager;
        }

        public T TransitionToState<T>() where T : GameState
        {
            var state = GetComponentInChildren<T>();

            if (!state)
            {
                var stateObj = new GameObject(typeof(T).Name);
                state = stateObj.AddComponent<T>();
                state.Init(this);
            }
            
            this.SetState(state);
            return state;
        }

	}
}