using UnityEngine;

using System;
using System.Collections.Generic;


namespace DSS.States
{
	public class GameStateManager : MonoBehaviour
	{
		public string Name { get; private set; }
		public GameStateManager Parent { get; private set; }

        [SerializeField]
		public GameState ActiveState;

        public event Action<GameState> StateTransition;

		public virtual void Init(string name, GameStateManager parent)
		{
			Name = name;
			Parent = parent;
            if (parent != null)
            {
                transform.parent = parent.transform;
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

		protected virtual void SetState(GameState state)
		{
            if( state != null && state.Manager != this )
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

        public T CreateSubManager<T>(string name) where T : GameStateManager
        {
            var managerObj = new GameObject(name);
            var manager = managerObj.AddComponent<T>();
            manager.Init(name, this);
            return manager;
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