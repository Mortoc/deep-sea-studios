using UnityEngine;

using System;
using System.Collections;

namespace Botter.States
{
	public class AppLoadingState : GameState
	{
        public event Action LoadingComplete;

		public override void EnterState()
		{
            base.EnterState();
            Application.LoadLevel("AppLoading");
		}

        public override void ExitState()
        {
            base.ExitState();
            LoadingComplete = null;
        }

        public void LoadingIsComplete()
        {
            if (LoadingComplete != null) LoadingComplete();
        }
	}
}