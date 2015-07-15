using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.Construction;

namespace DSS.States
{
    public class WorkshopState : GameState
    {
		public void Start()
		{
			EnterState (); // bootstrap cause this is the rootstate
		}

		public override void EnterState ()
		{
			base.EnterState ();

            this.Init(null);
            GoToState<HomeState>();            
        }

        public void GoToState<T>() where T : GameState
        {
            SetState(GetComponentsInChildren<T>(true)[0]);
        }
    }
}
