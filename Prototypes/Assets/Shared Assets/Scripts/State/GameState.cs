using UnityEngine;

using System;
using System.Collections.Generic;


namespace DSS.States
{
	public class GameState : MonoBehaviour
	{
		public GameState Parent { get; private set; }
        
		public GameState ActiveState { get; private set; }

        public event Action<GameState> StateTransition;

		public virtual void Init(GameState parent)
		{
			Parent = parent;
            if (parent != null)
            {
                transform.SetParent(parent.transform);
            }

            foreach (Transform child in transform)
            {
                foreach(var state in child.GetComponents<GameState>())
                {
                    state.Init(this);
                    state.gameObject.SetActive(false);
                }
            }
		}

        public event Action EnteredState;
        public event Action ExitedState;

        public virtual void EnterState()
        {
            if (EnteredState != null) EnteredState();
        }

        public virtual void ExitState()
        {
            if (ExitedState != null) ExitedState();
        }

        protected virtual void SetState(GameState state)
		{
            if( state != null && state.Parent != this )
            {
                throw new InvalidOperationException("Cannot go to foreign state");
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

            if (StateTransition != null) StateTransition(ActiveState);
		}
        
        public T TransitionToState<T>() where T : GameState
        {
            var state = default(T);
            foreach (Transform child in transform)
            {
                var childState = child.GetComponent<GameState>();
                if( childState is T )
                {
                    state = (T)childState;
                }
            }

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